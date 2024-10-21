using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Turns;

public class ChangeRotationBlock : StatementBlock
{
    public ChangeRotationBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }

    protected override async Task<Result> ExecuteAsyncCore()
    {
        try
        {
            if (Scope != null && Scope.TurnBlock is TurnByTurnBlock turnBlock)
            {
                turnBlock.ChangeRotation();
            }
            else
            {
                return Result.Failure($"Called {nameof(ChangeRotationBlock)} but turn block was null or not a turnByTurn Block");
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}