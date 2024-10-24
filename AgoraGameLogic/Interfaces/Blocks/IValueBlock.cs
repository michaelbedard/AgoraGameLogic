using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;

namespace AgoraGameLogic.Interfaces.Blocks;

public interface IValueBlock
{
    Result<T> GetValue<T>(IContext context);
    Result<object> GetValue(IContext context);
}