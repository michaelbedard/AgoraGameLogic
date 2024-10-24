using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Extensions;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Blocks.Dev;

/// <summary>
///
/// Use to define custom Condition Block
/// First arg is type, second is block definition
/// 
/// </summary>
public class DefineConditionBlockBlock : StatementBlock
{
    private string _customBlockType;
    private JArray _customBlockDefinition;
    private Block _customBlock;
    
    public DefineConditionBlockBlock(BlockBuildData buildData, GameData gameData) : base (buildData, gameData)
    {
        _customBlockType = buildData.Inputs[0].ToString();
        _customBlockDefinition = buildData.Inputs[1].AsValidArray();
        _customBlock = BlockFactory.CreateOrThrow<Block>(buildData.Inputs[2], gameData);
    }

    // register block
    protected override async Task<Result> ExecuteAsyncCore()
    {
        return BlockService.RegisterCustomBlock(_customBlockType, _customBlockDefinition, _customBlock);
    }
}