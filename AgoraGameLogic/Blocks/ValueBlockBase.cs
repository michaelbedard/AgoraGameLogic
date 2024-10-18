using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks;

public abstract class ValueBlockBase : BlockBase
{
    protected ValueBlockBase(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }
    
    public abstract Result<T> GetValue<T>(IContext context);

    public Result<object> GetValue(IContext context)
    {
        return GetValue<object>(context);
    }
}