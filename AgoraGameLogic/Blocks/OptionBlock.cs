using AgoraGameLogic.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks;

public class OptionBlock : Block
{
    public OptionBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        BlockType = BlockType.OptionBlock;
    }
}