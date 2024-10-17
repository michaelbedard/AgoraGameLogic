using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Blocks;

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