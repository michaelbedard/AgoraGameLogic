using System;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Interfaces.Other;
using AgoraGameLogic.Interfaces.Services;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Commands;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks;

/// <summary>
/// 
/// Statement blocks can trigger animations and events
/// 
/// </summary>
public abstract class StatementBlockBase : BlockBase
{
    private IAnimationService _animationService;
    private IActionService _actionService;
    private IInputService _inputService;
    private IEventService _eventService;
    
    public Scope? Scope;
    public TaskCompletionSource<bool> CompletionSource;
    
    protected StatementBlockBase(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        BlockType = BlockType.StatementBlock;
        
        _animationService = gameData.AnimationService;
        _actionService = gameData.ActionService;
        _inputService = gameData.InputService;
        _eventService = gameData.EventService;

        CompletionSource = new TaskCompletionSource<bool>();
    }
    
    // ABSTRACT

    protected abstract Task<Result> ExecuteAsync(IContext context);
    
    // METHODS
    
    #region EXECUTION
    
    public Task<Result> ExecuteAsync(IContext context, Scope? scope)
    {
        Scope = scope;
        return ExecuteAsync(context);
    }
    
    
    #endregion
    
    #region EVENTS

    public async Task<Result> TriggerEventsAsync<T>(IContext context, Command command) where T : EventBlockBase
    {
        return await _eventService.TriggerEventsAsync<T>(context, command, Scope);
    }

    #endregion
    
    #region ANIMATION

    public PendingRequest<AnimationCommand> PushAnimationOrThrow(AnimationCommand animationCommand)
    {
        return new PendingRequest<AnimationCommand>(animationCommand, _animationService, Players);
        
    }

    #endregion
    
    #region ACTION

    public PendingRequest<ActionCommand> PushActionOrThrow(ActionCommand actionCommand)
    {
        return new PendingRequest<ActionCommand>(actionCommand, _actionService, Players);
    }
    
    public PendingRequest<ActionCommand> PullActionOrThrow(ActionCommand actionCommand)
    {
        return new PendingRequest<ActionCommand>(actionCommand, _actionService, Players, false);
    }

    #endregion
    
    #region INPUT
    
    public PendingRequest<InputCommand> PushInputOrThrow(InputCommand inputCommand)
    {
        return new PendingRequest<InputCommand>(inputCommand, _inputService, Players);
    }
    
    public PendingRequest<InputCommand> PullInputOrThrow(InputCommand inputCommand)
    {
        return new PendingRequest<InputCommand>(inputCommand, _inputService, Players, false);
    }

    #endregion
    
    #region REVERT

    protected Result RegisterRevertibleBlock(IRevertible revertibleBlock)
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

    #region Sequence

    protected async Task<Result> ExecuteSequenceAsync(StatementBlockBase[] blocks, IContext context)
    {
        return await ExecuteSequenceAsync(blocks, context, Scope);
    }
    
    protected async Task ExecuteSequenceOrThrowAsync(StatementBlockBase[] blocks, IContext context)
    {
        await ExecuteSequenceOrThrowAsync(blocks, context, Scope);
    }

    #endregion
}