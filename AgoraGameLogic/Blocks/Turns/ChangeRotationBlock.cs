using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Turns;

public class ChangeRotationBlock : StatementBlockBase
{
    public ChangeRotationBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }

    protected override async Task<Result> ExecuteAsync(IContext context)
    {
        try
        {
            if (Scope != null && Scope.TurnBlock is TurnByTurnBlock turnBlock)
            {
                turnBlock.ChangeRotation();
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}