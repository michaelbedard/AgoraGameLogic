using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;

namespace AgoraGameLogic.Logic.Blocks;

public abstract class BaseValueBlock : BaseBlock
{
    protected BaseValueBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
    }
    
    public abstract T GetValue<T>(Context context);

    public object GetValue(Context context)
    {
        return GetValue<object>(context);
    }
}