using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Dtos;
using AgoraGameLogic.Interfaces.Actors;

namespace AgoraGameLogic.Utility.Commands;

public abstract class InputCommand : Command
{
    public abstract Task<Result> ResolveAsync(object? answer);
    public abstract Task<Result> ResolveDefaultAsync();
}

public abstract class InputCommand<TCommand, TBlock, TEvent> : InputCommand
    where TCommand : InputCommand<TCommand, TBlock, TEvent>
    where TBlock : InputBlock<TCommand, TBlock, TEvent>
    where TEvent : EventBlock
{
    public TBlock InputBlock;
    public TurnScope Scope;

    public InputCommand(TBlock inputBlock, TurnScope scope) 
    {
        InputBlock = inputBlock;
        Type = typeof(TBlock);
        Scope = scope;
    }

    public abstract Result Resolve(object? answer);
    public abstract object GetDefaultAnswer();
    public abstract Result Revert();
    public abstract CommandDto GetDtoCore();
    
    // wrapper
    public override async Task<Result> ResolveAsync(object? answer)
    {
        // resolve
        var performResult = Resolve(answer);
        if (!performResult.IsSuccess)
        {
            return Result.Failure(performResult.Error);
        }
        
        // events
        var eventResult = await InputBlock.TriggerEventsAsync<TEvent>((TCommand)this);
        if (!eventResult.IsSuccess)
        {
            return Result.Failure(eventResult.Error);
        }
        
        // continue execution
        InputBlock.ValidateCompletionSource();
        return Result.Success();
    }
    
    public override async Task<Result> ResolveDefaultAsync()
    {
        if (IsCancelable)
        {
            // should cancel
            throw new NotImplementedException();
        }

        var defaultAnswer = GetDefaultAnswer();
        return await ResolveAsync(defaultAnswer);
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
        var temp = GetDtoCore();
        temp.Key = nameof(TCommand);
        temp.Options = Options;

        return temp;
    }
}