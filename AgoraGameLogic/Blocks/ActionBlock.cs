using AgoraGameLogic.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Commands;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks;

public abstract class ActionBlock : StatementBlock
{
    public ActionBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }
}

public abstract class ActionBlock<TCommand, TBlock, TEvent> : ActionBlock
    where TCommand : ActionCommand<TCommand, TBlock, TEvent>
    where TBlock : ActionBlock<TCommand, TBlock, TEvent>
    where TEvent : EventBlock
{
    protected Value<ActionBehavior> BehaviorValue;
    protected ActionBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        BehaviorValue = Value<ActionBehavior>.ParseOrThrow(buildData.Inputs[0], gameData); // always in 1st place
    }

    protected abstract Result<TCommand> GetCommand();
    
    protected override async Task<Result> ExecuteAsyncCore()
    {
        try
        {
            var behavior= BehaviorValue.GetValueOrThrow(Context);
            var commandResult = GetCommand();
            if (!commandResult.IsSuccess)
            {
                return Result.Failure(commandResult.Error);
            }

            var command = commandResult.Value;
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
                    await command.PerformAsync(false);
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