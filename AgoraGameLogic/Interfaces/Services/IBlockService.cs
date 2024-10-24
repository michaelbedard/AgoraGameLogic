using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Services;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Interfaces.Services;

public interface IBlockService
{
    void RegisterDefinitionBlock(StatementBlock definitionBlock);
    List<StatementBlock> GetRegisterDefinitionBlocks();
    Result RegisterCustomBlock(string customBlockType, JArray customBlockDefinition, Block customBlock);
    Result<CustomBlockData> GetCustomBlock(string customBlockType);
}