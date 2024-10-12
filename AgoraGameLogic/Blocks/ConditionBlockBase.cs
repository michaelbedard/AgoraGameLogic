using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks;

public abstract class ConditionBlockBase : BlockBase
{
    protected ConditionBlockBase(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
    }
    
    public abstract bool IsSatisfied(IContext context);
}