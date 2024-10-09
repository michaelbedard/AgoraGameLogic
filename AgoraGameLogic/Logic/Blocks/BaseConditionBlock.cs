using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;

namespace AgoraGameLogic.Logic.Blocks;

public abstract class BaseConditionBlock : BaseBlock
{
    protected BaseConditionBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
    }
    
    public abstract bool IsSatisfied(Context context);
}