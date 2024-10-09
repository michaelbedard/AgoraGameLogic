using AgoraGameLogic.Core.Entities.Utility;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Enums;

namespace AgoraGameLogic.Logic.Blocks;

public abstract class BaseActionBlock : BaseStatementBlock
{
    public BaseActionBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
    }
}

public abstract class BaseActionBlock<TCommand, TBlock> : BaseActionBlock
    where TCommand : BaseActionCommand<TCommand, TBlock>
    where TBlock : BaseActionBlock<TCommand, TBlock>
{
    protected Value<ActionBehavior> BehaviorValue;
    
    protected BaseActionBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
    }

    public abstract TCommand GetCommand(Context context);
    
    public override async Task ExecuteAsync(Context context, Scope scope)
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