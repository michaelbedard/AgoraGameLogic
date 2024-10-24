using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;

namespace AgoraGameLogic.Interfaces.Blocks;

public interface IConditionBlock
{
    public Result<bool> IsSatisfied(IContext context);
    public bool IsSatisfiedOrThrow(IContext context);
}