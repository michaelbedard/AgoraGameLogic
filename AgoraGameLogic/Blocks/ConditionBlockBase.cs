using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Blocks;

public abstract class ConditionBlockBase : BlockBase
{
    protected ConditionBlockBase(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }
    
    public abstract Result<bool> IsSatisfied(IContext context);

    public bool IsSatisfiedOrThrow(IContext context)
    {
        var isSatisfiedResult = IsSatisfied(context);
        if (!isSatisfiedResult.IsSuccess)
        {
            throw new Exception(isSatisfiedResult.Error);
        }

        return isSatisfiedResult.Value;
    }
}