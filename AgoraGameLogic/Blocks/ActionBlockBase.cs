using AgoraGameLogic.Core.Entities.Utility;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks;

public abstract class ActionBlockBase : StatementBlockBase
{
    public ActionBlockBase(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
    }
}

public abstract class ActionBlockBase<TCommand, TBlock> : ActionBlockBase
    where TCommand : ActionCommandBase<TCommand, TBlock>
    where TBlock : ActionBlockBase<TCommand, TBlock>
{
    protected Value<ActionBehavior> BehaviorValue;
    
    protected ActionBlockBase(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
    }

    public abstract TCommand GetCommand(IContext context);
    
    public override async Task ExecuteAsync(IContext context, Scope scope)
    {
        var behavior= BehaviorValue.GetValue(context);
        var command = GetCommand(context);
        
        switch (behavior)
        {
            case ActionBehavior.Allow:
            {
                PushAction(command).For(command.Target);
                break;
            }
            case ActionBehavior.Disallow:
            {
                PushAction(command).For(command.Target);
                break;
            }
            case ActionBehavior.Perform:
            {
                await command.PerformAsync(context, false);
                break;
            }
        }
    }

}