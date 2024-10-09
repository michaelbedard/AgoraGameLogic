using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Logic.Blocks;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Domain.Entities.Models;

public abstract class BaseInputCommand : BaseCommand
{
    public abstract Task PerformAsync(Context context, object? answer, bool shouldRegisterAction);
}

public abstract class BaseInputCommand<TCommand, TBlock> : BaseInputCommand
    where TCommand : BaseInputCommand<TCommand, TBlock>
    where TBlock : BaseInputBlock<TCommand, TBlock>
{
    public TBlock InputBlock;

    public BaseInputCommand(TBlock inputBlock, Scope? scope) 
    {
        InputBlock = inputBlock;
        Type = typeof(TBlock);
        Scope = scope;
    }
    
    public abstract Task PerformAsync(TCommand command, Context context, object? answer);
    public abstract Task TriggerEventsAsync(TCommand command, Context context, object? answer);
    public abstract void Revert(TCommand command, Context context);
    public abstract bool Equals(TCommand inputCommand);
    public abstract BaseCommandDto GetDto(TCommand inputCommand);
    
    // wrapper
    public override async Task PerformAsync(Context context, object? answer, bool shouldRegisterAction)
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