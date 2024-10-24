using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;

namespace AgoraGameLogic.Interfaces.Services;

public interface IBlockService
{
    Result RegisterCustomBlock(string customBlockType, Block customBlock);
    Result<Block> GetCustomBlock(string customBlockType);
}