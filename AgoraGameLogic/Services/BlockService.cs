using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Interfaces.Services;

namespace AgoraGameLogic.Services;

public class BlockService : IBlockService
{
    private readonly Dictionary<string, Block> _customBlockByType = new Dictionary<string, Block>();

    public Result RegisterCustomBlock(string customBlockType, Block customBlock)
    {
        if (_customBlockByType.ContainsKey(customBlockType))
        {
            return Result.Failure($"Cannot register custom block {customBlockType} twice");
        }

        _customBlockByType[customBlockType] = customBlock;
        return Result.Success();
    }

    public Result<Block> GetCustomBlock(string customBlockType)
    {
        if (_customBlockByType.TryGetValue(customBlockType, out var customBlock))
        {
            return Result<Block>.Success(customBlock);
        }

        return Result<Block>.Failure($"Cannot get custom block {customBlockType}");
    }
}