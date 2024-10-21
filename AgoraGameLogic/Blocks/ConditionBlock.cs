using System;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks;

public abstract class ConditionBlock : Block
{
    protected ConditionBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        BlockType = BlockType.ConditionBlock;
    }
    
    protected abstract Result<bool> IsSatisfiedCore();
    
    public Result<bool> IsSatisfied(IContext context)
    {
        SetUpContext(context);
        return IsSatisfiedCore();
    }

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