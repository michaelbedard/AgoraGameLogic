using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Logic.Blocks;
using AgoraGameLogic.Logic.Blocks.Turns;

namespace AgoraGameLogic.Domain.Entities.Models;

public abstract class ActionCommandBase : CommandBase, IEquatable<ActionCommandBase>
{
    public abstract Task PerformAsync(IContext context, bool shouldRegisterAction);
    public abstract bool Equals(ActionCommandBase actionCommandBase);
        
}

public abstract class ActionCommandBase<TCommand, TBlock> : ActionCommandBase , IEquatable<TCommand>
    where TCommand : ActionCommandBase<TCommand, TBlock>
    where TBlock : ActionBlockBase<TCommand, TBlock>
{
    public TBlock ActionBlockStatementBlock;

    public ActionCommandBase(TBlock actionBlockStatementBlock, Scope? scope)
    {
        ActionBlockStatementBlock = actionBlockStatementBlock;
        Type = typeof(TBlock);
        Scope = scope;
    }
    
    public abstract void Perform(TCommand command, IContext context);
    public abstract void Revert(TCommand command, IContext context);
    public abstract bool Equals(TCommand command);
    public abstract BaseCommandDto GetDto(TCommand command);
    
    // wrapper
    public override async Task PerformAsync(IContext context, bool shouldRegisterAction)
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
    public override bool Equals(ActionCommandBase command)
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