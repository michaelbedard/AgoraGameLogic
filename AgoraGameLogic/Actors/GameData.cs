using System;
using System.Collections.Generic;
using AgoraGameLogic.Dtos;
using AgoraGameLogic.Interfaces.Services;
using AgoraGameLogic.Services;

namespace AgoraGameLogic.Actors;

public class GameData
{
    public Action<StateChangeDto> OnGameStateChange = delegate { };
    public Action<EndGameDto> OnEndGame = delegate { };
    
    public EventService EventService = new EventService();
    public IActionService ActionService = new ActionService();
    public IInputService InputService = new InputService();
    public ExecutionService ExecutionService = new ExecutionService();
    public AnimationService AnimationService = new AnimationService();
    public DescriptionService DescriptionService = new DescriptionService();
    public ScoringService ScoringService = new ScoringService();
    
    public Context GlobalContext = new Context();
    public List<GameModule> Players = new List<GameModule>();
    public int NumberOfPlayers;
    
    public bool GameIsRunning = false;

    public GameData()
    {
        OnEndGame += r =>
        {
            GameIsRunning = false;
        };
    }
}