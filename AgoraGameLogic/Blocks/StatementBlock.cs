using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Interfaces.Other;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Commands;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks;

/// <summary>
/// 
/// Statement blocks can trigger animations and events
/// 
/// </summary>
public abstract class StatementBlock : Block
{
    private TaskCompletionSource<bool> _completionSource;
    private Dictionary<string, TaskCompletionSource<bool>> _completionSourceByKey;
    
    protected StatementBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        BlockType = BlockType.StatementBlock;
        
        _completionSource = new TaskCompletionSource<bool>();
        _completionSourceByKey = new Dictionary<string, TaskCompletionSource<bool>>();
    }
    
    // ABSTRACT

    protected abstract Task<Result> ExecuteAsyncCore();
    
    // METHODS
    
    #region EXECUTION
    
    public Task<Result> ExecuteAsync(IContext context, TurnScope? scope)
    {
        SetUpContext(context);
        SetUpScope(scope);
        
        return ExecuteAsyncCore();
    }
    
    
    #endregion
    
    #region EVENTS

    public async Task<Result> TriggerEventsAsync<T>(Command command) where T : EventBlock
    {
        return await EventService.TriggerEventsAsync<T>(Scope, command);
    }

    #endregion
    
    #region ANIMATION

    public PendingRequest<AnimationCommand> PushAnimationOrThrow(AnimationCommand animationCommand)
    {
        return new PendingRequest<AnimationCommand>(animationCommand, AnimationService, Players);
        
    }

    #endregion
    
    #region ACTION

    public PendingRequest<ActionCommand> PushActionOrThrow(ActionCommand actionCommand)
    {
        return new PendingRequest<ActionCommand>(actionCommand, ActionService, Players);
    }
    
    public PendingRequest<ActionCommand> PullActionOrThrow(ActionCommand actionCommand)
    {
        return new PendingRequest<ActionCommand>(actionCommand, ActionService, Players, false);
    }

    #endregion
    
    #region INPUT
    
    public PendingRequest<InputCommand> PushInputOrThrow(InputCommand inputCommand)
    {
        return new PendingRequest<InputCommand>(inputCommand, InputService, Players);
    }
    
    public PendingRequest<InputCommand> PullInputOrThrow(InputCommand inputCommand)
    {
        return new PendingRequest<InputCommand>(inputCommand, InputService, Players, false);
    }

    #endregion
    
    #region REVERT

    protected Result RegisterRevertibleBlock(IRevertible revertibleBlock)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region AWAIT

    public TaskCompletionSource<bool> GetOrCreateCompletionSource()
    {
        return _completionSource;
    }
    
    public TaskCompletionSource<bool> GetOrCreateCompletionSource(string key)
    {
        if (!_completionSourceByKey.ContainsKey(key))
        {
            _completionSourceByKey[key] = new TaskCompletionSource<bool>();
        }
        
        return _completionSourceByKey[key];
    }

    public void ValidateCompletionSource()
    {
        var completionSource = GetOrCreateCompletionSource();
        if (!completionSource.Task.IsCompleted)
        {
            completionSource.SetResult(true);
        }
    }
    
    public void ValidateCompletionSource(string key)
    {
        var completionSource = GetOrCreateCompletionSource(key);
        if (!completionSource.Task.IsCompleted)
        {
            completionSource.SetResult(true);
        }
    }
    
    public void ResetCompletionSource()
    {
        _completionSource = new TaskCompletionSource<bool>();
    }
    
    public void ResetCompletionSource(string key)
    {
        _completionSourceByKey[key] = new TaskCompletionSource<bool>();
    }

    #endregion
}