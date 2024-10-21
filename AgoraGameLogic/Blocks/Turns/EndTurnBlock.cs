using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Turns;

public class EndTurnBlock : StatementBlock
{
    public EndTurnBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }

    protected override async Task<Result> ExecuteAsyncCore()
    {
        if (Scope.TurnBlock != null)
        {
            return Scope.TurnBlock.EndCurrentTurn(Scope.Player);
        }

        return Result.Failure($"Called {nameof(EndTurnBlock)} but was not inside turn");
    }
}