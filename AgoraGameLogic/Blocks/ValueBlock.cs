using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks;

public abstract class ValueBlock : Block
{
    protected ValueBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        BlockType = BlockType.ValueBlock;
    }
    
    protected abstract Result<T> GetValue<T>();

    public Result<T> GetValue<T>(IContext context)
    {
        SetUpContext(context);
        return GetValue<T>();
    }
    
    public Result<object> GetValue(IContext context)
    {
        SetUpContext(context);
        return GetValue<object>();
    }
}