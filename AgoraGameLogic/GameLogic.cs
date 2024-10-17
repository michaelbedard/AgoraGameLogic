using AgoraGameLogic.Control.GameLoader;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Blocks.Game;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic;

public class GameLogic
{
    // GAME DATA
    
    private GameData _gameData = new GameData();
    
    // PROPERTIES
    
    public event Action<StateChangeDto> OnGameStateChange
    {
        add => _gameData.OnGameStateChange += value;
        remove => _gameData.OnGameStateChange -= value;
    }

    public event Action<EndGameDto> OnEndGame
    {
        add => _gameData.OnEndGame += value;
        remove => _gameData.OnEndGame -= value;
    }

    // METHODS
    
    public string LoadGame(string gameBuild, int numberOfPlayers)
    {
        _gameData.NumberOfPlayers = numberOfPlayers;
        var gameLoader = new GameLoader();
        
        // parse
        var buildJObject = JObject.Parse(gameBuild);
        var gameBuildDataResult = GameBuildData.Parse(buildJObject);
        if (!gameBuildDataResult.IsSuccess)
        {
            throw new Exception($"Error parsing game build: {gameBuildDataResult.Error}");
        }
        
        // load gameModules
        var gameBuildData = gameBuildDataResult.Value;
        var gameModulesToBuildDataResult = gameLoader.LoadGameModules(gameBuildData.GameModules, gameBuildData.Structures, _gameData);
        if (!gameModulesToBuildDataResult.IsSuccess)
        {
            throw new Exception(gameBuildDataResult.Error);
        }
        
        // set players
        var gameModulesToBuildData = gameModulesToBuildDataResult.Value;
        _gameData.Players = gameModulesToBuildData.Keys.Where(gm => gm.Type == GameModuleType.Player);
        
        // load descriptions, game module events, global variables, global events and scoring rules
        var loadResult = gameLoader.LoadDescriptions(gameModulesToBuildData, gameBuildData.Structures, _gameData)
            .Then(() => gameLoader.LoadGameModuleEvents(gameModulesToBuildData, gameBuildData.Structures, _gameData))
            .Then(() => gameLoader.LoadGlobalVariables(gameBuildData.GlobalVariables, _gameData))
            .Then(() => gameLoader.LoadGlobalEvents(gameBuildData.GlobalBlocks, _gameData))
            .Then(() => gameLoader.LoadScoringRules(gameBuildData.ScoringRules, _gameData));

        if (!loadResult.IsSuccess)
        {
            throw new Exception($"Error loading game: {loadResult.Error}");
        }

        // log
        Console.WriteLine(_gameData.GlobalContext.ToString());

        return "";
    }
    
    public void StartGame()
    {
        _gameData.GameIsRunning = true;
        
        Task.Run(async () =>
        {
            // perform action
            var result = await _gameData.EventService.TriggerEventsAsync<OnStartGameBlock>(_gameData.GlobalContext.Copy(), new StartGameCommand(), null);
            if (!result.IsSuccess)
            {
                Console.WriteLine($"Start game failed: {result.Error}");
            }
        });
        
        InvokeOnGameStateChange();
    }
    
    public void PerformAction(string playerId, int actionCommandId)
    {
        if (!_gameData.GameIsRunning) return;
        
        Task.Run(async () =>
        {
            // perform action
            var result = await _gameData.ActionService
                .PerformActionAsync(_gameData.GlobalContext.Copy(), playerId, actionCommandId);

            if (!result.IsSuccess)
            {
                Console.WriteLine($"Action failed: {result.Error}");
            }
        });
        
        InvokeOnGameStateChange();
    }
    
    public void PerformInput(string playerId, int inputCommandId, object? actionArg)
    {
        if (!_gameData.GameIsRunning) return;
        
        // _gameData.InputServiceBase.PerformInput(_gameData.GlobalContext.Copy(), playerId, inputCommandId, actionArg);
        InvokeOnGameStateChange();
    }

    private void InvokeOnGameStateChange()
    {
        // get results
        var animationsResult = _gameData.AnimationService.GetDtos();
        if (!animationsResult.IsSuccess)
        {
            throw new Exception(animationsResult.Error);
        }
        
        var actionsResult = _gameData.ActionService.GetDtos();
        if (!actionsResult.IsSuccess)
        {
            throw new Exception(actionsResult.Error);
        }
        
        var inputsResult = _gameData.InputService.GetDtos();
        if (!inputsResult.IsSuccess)
        {
            throw new Exception(inputsResult.Error);
        }
        
        var descriptionsResult = _gameData.DescriptionService.GetDescriptionDtos(_gameData.GlobalContext);
        if (!descriptionsResult.IsSuccess)
        {
            throw new Exception(descriptionsResult.Error);
        }
        
        
        // build state change Dto and invoke 
        _gameData.OnGameStateChange.Invoke(new StateChangeDto()
        {
            Animations = animationsResult.Value,
            Actions = actionsResult.Value,
            Inputs = inputsResult.Value,
            Descriptions = descriptionsResult.Value
        });
    }
}