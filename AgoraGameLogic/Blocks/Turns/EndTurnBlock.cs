using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Turns;

public class EndTurnBlock : StatementBlockBase
{
    public EndTurnBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }

    protected override Task<Result> ExecuteAsync(IContext context)
    {
        Scope?.TurnBlock.EndCurrentTurn(Scope.PlayerId);
    }
}