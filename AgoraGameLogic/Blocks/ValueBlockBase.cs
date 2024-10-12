using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks;

public abstract class ValueBlockBase : BlockBase
{
    protected ValueBlockBase(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
    }
    
    public abstract T GetValue<T>(IContext context);

    public object GetValue(IContext context)
    {
        return GetValue<object>(context);
    }
}