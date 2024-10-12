using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Logic.Blocks;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Domain.Entities.Models;

public abstract class InputCommandBase : CommandBase
{
    public abstract Task PerformAsync(IContext context, object? answer, bool shouldRegisterAction);
}

public abstract class InputCommandBase<TCommand, TBlock> : InputCommandBase
    where TCommand : InputCommandBase<TCommand, TBlock>
    where TBlock : InputBlockBase<TCommand, TBlock>
{
    public TBlock InputBlock;

    public InputCommandBase(TBlock inputBlock, Scope? scope) 
    {
        InputBlock = inputBlock;
        Type = typeof(TBlock);
        Scope = scope;
    }
    
    public abstract Task PerformAsync(TCommand command, IContext context, object? answer);
    public abstract Task TriggerEventsAsync(TCommand command, IContext context, object? answer);
    public abstract void Revert(TCommand command, IContext context);
    public abstract bool Equals(TCommand inputCommand);
    public abstract BaseCommandDto GetDto(TCommand inputCommand);
    
    // wrapper
    public override async Task PerformAsync(IContext context, object? answer, bool shouldRegisterAction)
    {
        var command = (TCommand)this;
        
        // logic and events
        await PerformAsync(command, context, answer);
        await TriggerEventsAsync(command, context, answer);
        
        // continue execution
        InputBlock.ValidateCompletionSource();
    }

    public override BaseCommandDto GetDto()
    {
        var temp = GetDto((TCommand)this);
        temp.Key = nameof(TCommand);
        temp.Options = Options;

        return temp;
    }
}