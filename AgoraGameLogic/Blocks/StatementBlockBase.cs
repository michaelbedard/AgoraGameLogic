using AgoraGameLogic.Control.Services;
using AgoraGameLogic.Core.Entities.Utility;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Logic.Blocks;

/// <summary>
/// 
/// Statement blocks can trigger animations and events
/// 
/// </summary>
public abstract class StatementBlockBase : BlockBase
{
    private AnimationService _animationService;
    private ActionService _actionService;
    private InputService _inputService;
    private EventService _eventService;
    
    public Scope? Scope;
    public TaskCompletionSource<bool> CompletionSource;
    
    protected StatementBlockBase(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        BlockType = BlockType.StatementBlock;
        
        _animationService = gameData.AnimationService;
        _actionService = gameData.ActionService;
        _inputService = gameData.InputService;
        _eventService = gameData.EventService;

        CompletionSource = new TaskCompletionSource<bool>();
    }
    
    // ABSTRACT

    public abstract Task ExecuteAsync(IContext context, Scope? scope);
    
    // METHODS
    
    #region EVENTS

    public async Task TriggerEventsAsync<T>(IContext context, object[] args) where T : EventBlockBase
    {
        await _eventService.TriggerEventsAsync<T>(context, args, Scope);
    }

    #endregion
    
    #region ANIMATION

    public PendingRequest<AnimationCommandBase> PushAnimation(AnimationCommandBase animationCommandBase)
    {
        return new PendingRequest<AnimationCommandBase>(animationCommandBase, _animationService, Players);
        
    }

    #endregion
    
    #region ACTION

    public PendingRequest<ActionCommandBase> PushAction(ActionCommandBase actionCommandBase)
    {
        return new PendingRequest<ActionCommandBase>(actionCommandBase, _actionService, Players);
    }
    
    public PendingRequest<ActionCommandBase> PullAction(ActionCommandBase actionCommandBase)
    {
        return new PendingRequest<ActionCommandBase>(actionCommandBase, _actionService, Players, false);
    }

    #endregion
    
    #region INPUT
    
    public PendingRequest<InputCommandBase> PushInput(InputCommandBase inputCommandBase)
    {
        return new PendingRequest<InputCommandBase>(inputCommandBase, _inputService, Players);
    }
    
    public PendingRequest<InputCommandBase> PullInput(InputCommandBase inputCommandBase)
    {
        return new PendingRequest<InputCommandBase>(inputCommandBase, _inputService, Players, false);
    }

    #endregion
    
    #region REVERT

    protected void RegisterRevertibleBlock(IRevertible revertibleBlock)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region AWAIT

    public void ValidateCompletionSource()
    {
        if (!CompletionSource.Task.IsCompleted)
        {
            CompletionSource.SetResult(true);
        }
    }
    
    public void ResetCompletionSource()
    {
        CompletionSource = new TaskCompletionSource<bool>();
    }

    #endregion
    
    
    
    
    // private Context? _blockContext; // Necessary for null-checks
    //
    // protected Context BlockContext
    // {
    //     get
    //     {
    //         if (_blockContext == null)
    //         {
    //             throw new InvalidOperationException("Context is not initialized.");
    //         }
    //         return _blockContext;
    //     }
    //     set
    //     {
    //         if (value == null)
    //         {
    //             throw new ArgumentNullException(nameof(value), "Context cannot be null.");
    //         }
    //         _blockContext = value;
    //     }
    // }
    //
    // protected ExecutionThread? BlockThread;
    // protected ExecutionScope? BlockScope;
    
    // public void SetContext(Context context)
    // {
    //     BlockContext = context;
    // }
    //
    // public void SetThread(ExecutionThread? thread)
    // {
    //     BlockThread = thread;
    // }
    //
    // public void SetScope(ExecutionScope? scope)
    // {
    //     BlockScope = scope;
    // }
    //
    // public ExecutionScope? GetScope()
    // {
    //     return BlockScope;
    // }
    
    public virtual void OnShallowCopy()
    {
    }
}