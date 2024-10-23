using AgoraGameLogic.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Dev;

public class DefineOperatorBlockBlock : StatementBlock
{
    public DefineOperatorBlockBlock(BlockBuildData buildData, GameData gameData) : base (buildData, gameData)
    {
        
    }

    // register block
    protected override async Task<Result> ExecuteAsyncCore()
    {
        return Result.Success();
    }
}