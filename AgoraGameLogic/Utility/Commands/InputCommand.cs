using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Blocks;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Domain.Entities.Models;

public abstract class InputCommand : Command
{
    public abstract Task<Result> PerformAsync(IContext context, object? answer, bool shouldRegisterAction);
}

public abstract class InputCommand<TCommand, TBlock, TEvent> : InputCommand
    where TCommand : InputCommand<TCommand, TBlock, TEvent>
    where TBlock : InputBlockBase<TCommand, TBlock, TEvent>
    where TEvent : EventBlockBase
{
    public TBlock InputBlock;

    public InputCommand(TBlock inputBlock, Scope? scope) 
    {
        InputBlock = inputBlock;
        Type = typeof(TBlock);
        Scope = scope;
    }
    
    public abstract Task<Result> PerformAsync(TCommand command, IContext context, object? answer);
    public abstract Result Revert(TCommand command, IContext context);
    public abstract CommandDto InitializeDto();
    
    // wrapper
    public override async Task<Result> PerformAsync(IContext context, object? answer, bool shouldRegisterAction)
    {
        var command = (TCommand)this;
        
        // perform
        var performResult = await PerformAsync(command, context, answer);
        if (!performResult.IsSuccess)
        {
            return Result.Failure(performResult.Error);
        }
        
        // events
        var eventResult = await InputBlock.TriggerEventsAsync<TEvent>(context, command);
        if (!eventResult.IsSuccess)
        {
            return Result.Failure(eventResult.Error);
        }
        
        // continue execution
        InputBlock.ValidateCompletionSource();
        return Result.Success();
    }
    
    /// <summary>
    /// Equals
    /// </summary>
    public override bool Equals(Command command)
    {
        if (command.Type == typeof(TBlock) && Target == command.Target)
        {
            return Equals((TCommand)command);
        }

        return false;
    }
    
    public override CommandDto GetDto()
    {
        var temp = GetDto();
        temp.Key = nameof(TCommand);
        temp.Options = Options;

        return temp;
    }
}