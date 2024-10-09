using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Logic.Blocks;
using AgoraGameLogic.Logic.Blocks.Turns;

namespace AgoraGameLogic.Domain.Entities.Models;

public abstract class BaseActionCommand : BaseCommand, IEquatable<BaseActionCommand>
{
    public abstract Task PerformAsync(Context context, bool shouldRegisterAction);
    public abstract bool Equals(BaseActionCommand baseActionCommand);
        
}

public abstract class BaseActionCommand<TCommand, TBlock> : BaseActionCommand , IEquatable<TCommand>
    where TCommand : BaseActionCommand<TCommand, TBlock>
    where TBlock : BaseActionBlock<TCommand, TBlock>
{
    public TBlock ActionBlock;

    public BaseActionCommand(TBlock actionBlock, Scope? scope)
    {
        ActionBlock = actionBlock;
        Type = typeof(TBlock);
        Scope = scope;
    }
    
    public abstract void Perform(TCommand command, Context context);
    public abstract void Revert(TCommand command, Context context);
    public abstract bool Equals(TCommand command);
    public abstract BaseCommandDto GetDto(TCommand command);
    
    // wrapper
    public override async Task PerformAsync(Context context, bool shouldRegisterAction)
    {
        Perform((TCommand)this, context);
        
        // events

        // check if shouldRegisterAction, and that turn block is in scope
        if (shouldRegisterAction && Scope != null && Scope.TurnBlock is TurnByTurnBlock turnByTurnBlock)
        {
            turnByTurnBlock.RegisterActionCount(Target);
        }
    }

    // wrapper
    public override bool Equals(BaseActionCommand command)
    {
        if (command.Type == typeof(TBlock) && Target == command.Target)
        {
            return Equals((TCommand)command);
        }

        return false;
    }
    
    public override BaseCommandDto GetDto()
    {
        var temp = GetDto((TCommand)this);
        temp.Key = nameof(TCommand);
        temp.Options = Options;
        temp.TargetId = Target?.Id;

        return temp;
    }
}