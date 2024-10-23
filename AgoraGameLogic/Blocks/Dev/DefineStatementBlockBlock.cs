using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Utility.BuildData;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Blocks.Dev;

public class DefineStatementBlockBlock : StatementBlock
{
    public DefineStatementBlockBlock(BlockBuildData buildData, GameData gameData) : base (buildData, gameData)
    {
        
    }

    // register block
    protected override async Task<Result> ExecuteAsyncCore()
    {
        return Result.Success();
    }
}