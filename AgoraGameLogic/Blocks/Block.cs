using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Interfaces.Services;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks;

public abstract class Block
{
    protected IActionService ActionService;
    protected IAnimationService AnimationService;
    protected IEventService EventService;
    protected IExecutionService ExecutionService;
    protected IInputService InputService;
    protected IScoringService ScoringService;
    
    protected BlockType BlockType;
    protected OptionBlock[] Options;
    protected List<GameModule> Players;

    public IContext Context { get; set; }
    public TurnScope? Scope { get; set; }
    
    public Block(BlockBuildData buildData, GameData gameData)
    {
        ActionService = gameData.ActionService;
        AnimationService = gameData.AnimationService;
        EventService = gameData.EventService;
        ExecutionService = gameData.ExecutionService;
        InputService = gameData.InputService;
        ScoringService = gameData.ScoringService;
        
        Options = BlockFactory.CreateArrayOrThrow<OptionBlock>(buildData.Options, gameData);
        Players = gameData.Players;
    }
    
    // SETUP

    protected void SetUpContext(IContext context)
    {
        Context = context;
    }
    
    protected void SetUpScope(TurnScope? scope)
    {
        Scope = scope;
    }
    
    // OPTIONS
    
    public bool HasOption<T>() where T : OptionBlock
    {
        return Options.Any(option => option is T);
    }

    public IEnumerable<T> GetOptionsOrThrow<T>() where T : OptionBlock
    {
        return Options.OfType<T>();
    }

    public T GetOptionOrThrow<T>() where T : OptionBlock
    {
        var options = GetOptionsOrThrow<T>();
        if (options.Count() == 0)
        {
            throw new Exception($"block has option {typeof(T)}");
        }

        return GetOptionsOrThrow<T>().ToList()[0];
    }

    // SEQUENCE
    
    protected async Task<Result> ExecuteSequenceAsync(StatementBlock[] blocks, IContext? context = null, TurnScope? scope = null)
    {
        foreach (var block in blocks)
        {
            context = context ?? Context;
            scope = scope ?? Scope;
            
            var executeResult = await block.ExecuteAsync(context, scope);
            if (!executeResult.IsSuccess)
            {
                return Result.Failure(executeResult.Error);
            }
        }
        
        return Result.Success();
    }
    
    protected async Task ExecuteSequenceOrThrowAsync(StatementBlock[] blocks, IContext? context = null, TurnScope? scope = null)
    {
        var executeSequenceResult = await ExecuteSequenceAsync(blocks, context, scope);
        if (!executeSequenceResult.IsSuccess)
        {
            throw new Exception(executeSequenceResult.Error);
        }
    }
}