using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Interfaces.Services;
using AgoraGameLogic.Utility.Extensions;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Services;

public enum CustomBlockSegmentType
{
    Text,
    Boolean,
    Value
}

public class BlockService : IBlockService
{
    private readonly Dictionary<string, CustomBlockData> _customBlockByType = new Dictionary<string, CustomBlockData>();
    private readonly List<StatementBlock> _customDefinitionBlocks = new  List<StatementBlock>();
    
    public void RegisterDefinitionBlock(StatementBlock definitionBlock)
    {
        _customDefinitionBlocks.Add(definitionBlock);
    }
    
    public List<StatementBlock> GetRegisterDefinitionBlocks()
    {
        return _customDefinitionBlocks;
    }

    public Result RegisterCustomBlock(string customBlockType, JArray customBlockDefinition, Block customBlock)
    {
        if (_customBlockByType.ContainsKey(customBlockType))
        {
            return Result.Failure($"Cannot register custom block {customBlockType} twice");
        }

        var segments = new List<CustomBlockData.CustomBlockDefinitionSegment>();
        foreach (var customBlockSegementToken in customBlockDefinition)
        {
            var segmentResult = CustomBlockData.CustomBlockDefinitionSegment.Parse(customBlockSegementToken);
            if (!segmentResult.IsSuccess)
            {
                return Result.Failure(segmentResult.Error);
            }
            
            segments.Add(segmentResult.Value);
        }

        // bind custom block data to dictionary
        _customBlockByType[customBlockType] = new CustomBlockData()
        {
            Definition = segments,
            CustomBlock = customBlock
        };
        
        return Result.Success();
    }

    public Result<CustomBlockData> GetCustomBlock(string customBlockType)
    {
        if (_customBlockByType.TryGetValue(customBlockType, out var customBlock))
        {
            return Result<CustomBlockData>.Success(customBlock);
        }

        return Result<CustomBlockData>.Failure($"Cannot get custom block {customBlockType}");
    }
}

public class CustomBlockData
{
    public List<CustomBlockDefinitionSegment> Definition { get; set; }
    public Block CustomBlock { get; set; }
        
    public class CustomBlockDefinitionSegment
    {
        public CustomBlockSegmentType SegmentType { get; set; }
        public string SegmentLabel { get; set; }

        public static Result<CustomBlockDefinitionSegment> Parse(JToken token)
        {
            return token.TryGetValue<CustomBlockDefinitionSegment>();
        }
    }
}