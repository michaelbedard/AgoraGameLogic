using AgoraGameLogic.Control.GameLoader;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Extensions;
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
        var buildDefinition = BuildDefinition.Parse(buildJObject);
        
        // load gameModules
        var gameModulesToDefinition = gameLoader.LoadGameModules(
            buildDefinition.GameModules, 
            buildDefinition.Structures, 
            _gameData);
        
        // set players
        _gameData.Players = gameModulesToDefinition.Keys.Where(gm => gm.Type == GameModuleType.Player);
        
        // load global variables, events and scoring rules
        gameLoader.LoadDescriptions(gameModulesToDefinition, buildDefinition.Structures, _gameData);
        gameLoader.LoadGameModuleEvents(gameModulesToDefinition, buildDefinition.Structures, _gameData);
        gameLoader.LoadGlobalVariables(buildDefinition.GlobalVariables, _gameData); 
        gameLoader.LoadGlobalEvents(buildDefinition.GlobalBlocks, _gameData);
        gameLoader.LoadScoringRules(buildDefinition.ScoringRules, _gameData);

        // log
        Console.WriteLine(_gameData.GlobalContext.ToString());
        
        _gameData.GlobalContext.Get<GameModule>("DrawingDeck").Fields.Get<List<GameModule>>("Cards").PrintToConsole();

        return "";
    }
    
    public void StartGame()
    {
        _gameData.GameIsRunning = true;
        
        _gameData.EventService.TriggerEvents<OnStartGameBlock>(_gameData.GlobalContext.Copy(), Array.Empty<object>(), null);
        InvokeOnGameStateChange();
    }
    
    public void PerformAction(string playerId, int actionCommandId)
    {
        if (!_gameData.GameIsRunning) return;
        
        _gameData.ActionService.PerformAction(_gameData.GlobalContext.Copy(), playerId, actionCommandId);
        InvokeOnGameStateChange();
    }
    
    public void PerformInput(string playerId, int inputCommandId, object? actionArg)
    {
        if (!_gameData.GameIsRunning) return;
        
        _gameData.InputService.PerformInput(_gameData.GlobalContext.Copy(), playerId, inputCommandId, actionArg);
        InvokeOnGameStateChange();
    }

    private void InvokeOnGameStateChange()
    {
        _gameData.OnGameStateChange.Invoke(new StateChangeDto()
        {
            Animations = _gameData.AnimationService.GetDtos(),
            Actions = _gameData.ActionService.GetDtos(),
            Inputs = _gameData.InputService.GetDtos(),
            Descriptions = _gameData.DescriptionService.GetDescriptionDtos(_gameData.GlobalContext)
        });
    }
}