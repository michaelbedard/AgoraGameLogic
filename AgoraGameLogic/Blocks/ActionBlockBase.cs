using System;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Commands;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks;

public abstract class ActionBlockBase : StatementBlockBase
{
    public ActionBlockBase(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }
}

public abstract class ActionBlockBase<TCommand, TBlock, TEvent> : ActionBlockBase
    where TCommand : ActionCommand<TCommand, TBlock, TEvent>
    where TBlock : ActionBlockBase<TCommand, TBlock, TEvent>
    where TEvent : EventBlockBase
{
    protected Value<ActionBehavior> BehaviorValue;
    protected ActionBlockBase(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        BehaviorValue = Value<ActionBehavior>.ParseOrThrow(buildData.Inputs[0], gameData); // always in 1st place
    }

    public abstract TCommand GetCommandOrThrow(IContext context);
    
    protected override async Task<Result> ExecuteAsync(IContext context)
    {
        try
        {
            var behavior= BehaviorValue.GetValueOrThrow(context);
            var command = GetCommandOrThrow(context);
        
            switch (behavior)
            {
                case ActionBehavior.Allow:
                {
                    PushActionOrThrow(command).For(command.Target);
                    break;
                }
                case ActionBehavior.Disallow:
                {
                    PullActionOrThrow(command).For(command.Target);
                    break;
                }
                case ActionBehavior.Perform:
                {
                    await command.PerformAsync(context, false);
                    break;
                }
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

}