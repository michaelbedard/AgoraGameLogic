using AgoraGameLogic.Control.Services;
using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic;

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
    public IEnumerable<GameModule> Players = new GameModule[]{};
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