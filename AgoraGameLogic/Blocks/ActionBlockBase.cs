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
    
    public override async Task<Result> ExecuteAsync(Scope scope)
    {
        try
        {
            var behavior= BehaviorValue.GetValueOrThrow(scope.Context);
            var command = GetCommandOrThrow(scope.Context);
        
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
                    await command.PerformAsync(scope.Context, false);
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