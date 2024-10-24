﻿using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks.Game.StartGame;
using AgoraGameLogic.Dtos;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;
using AgoraGameLogic.Utility.Extensions;
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
        var gameLoader = new GameLoader.GameLoader();
        
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
        _gameData.Players = gameModulesToBuildData.Keys.Where(gm => gm.Type == GameModuleType.Player).ToList();
        
        // load descriptions, game module events, global variables, global events and scoring rules
        var loadResult = gameLoader.LoadDescriptions(gameModulesToBuildData, gameBuildData.Structures, _gameData)
            .Then(() => gameLoader.LoadGameModuleEvents(gameModulesToBuildData, gameBuildData.Structures, _gameData))
            .Then(() => gameLoader.LoadGlobalVariables(gameBuildData.GlobalVariables, _gameData))
            .Then(() => gameLoader.LoadGlobalBlocks(gameBuildData.GlobalBlocks, _gameData))
            .Then(() => gameLoader.LoadScoringRules(gameBuildData.ScoringRules, _gameData));
        if (!loadResult.IsSuccess)
        {
            throw new Exception($"[Error loading game] {loadResult.Error}");
        }
        
        // setup context inside services
        _gameData.EventService.SetGlobalContext(_gameData.GlobalContext);
        _gameData.ActionService.SetGlobalContext(_gameData.GlobalContext);
        _gameData.InputService.SetGlobalContext(_gameData.GlobalContext);
        _gameData.AnimationService.SetGlobalContext(_gameData.GlobalContext);
        
        // initialize services
        var initializeResult = _gameData.InputService.InitializeDictionaryEntries(_gameData.Players)
            .Then(() => _gameData.ActionService.InitializeDictionaryEntries(_gameData.Players))
            .Then(() => _gameData.AnimationService.InitializeDictionaryEntries(_gameData.Players));
        if (!initializeResult.IsSuccess)
        {
            throw new Exception($"[Error initializing dictionnaries] {initializeResult.Error}");
        }

        // log
        // Console.WriteLine(_gameData.GlobalContext.ToString());

        return "";
    }
    
    public void StartGame()
    {
        _gameData.GameIsRunning = true;
        
        // call all custom definiton block
        foreach (var customBlockDefinitionBlock in _gameData.BlockService.GetRegisterDefinitionBlocks())
        {
            var definitionTask = customBlockDefinitionBlock.ExecuteAsync(_gameData.GlobalContext.Copy(), null);
            definitionTask.ContinueWith(t =>
            {
                if (t.Result != null && !t.Result.IsSuccess)
                {
                    Console.WriteLine($"[Custom block definition failed] {t.Result.Error}");
                }
            });
        }
        
        // trigger start game with empty command
        var startGameTask = _gameData.EventService.TriggerEventsAsync<OnStartGameBlock>(null, new StartGameCommand());
        startGameTask.ContinueWith(t =>
        {
            if (t.Result != null && !t.Result.IsSuccess)
            {
                Console.WriteLine($"[Start game failed] {t.Result.Error}");
            }
        });
        
        InvokeOnGameStateChange();
    }
    
    public void PerformAction(string playerId, int actionCommandId)
    {
        if (!_gameData.GameIsRunning) return;

        if (string.IsNullOrEmpty(playerId))
        {
            Console.WriteLine($"[Action failed] PlayerId canot be null or empty");
            return;
        }
        
        // perform action
        var task = _gameData.ActionService.PerformActionAsync(playerId, actionCommandId);

        task.ContinueWith(t =>
        {
            if (t.Result != null && !t.Result.IsSuccess)
            {
                Console.WriteLine($"[Action failed] {t.Result.Error}");
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