using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;

namespace AgoraGameLogic.Interfaces.Blocks;

public interface IStatementBlock
{
    Task<Result> ExecuteAsync(IContext context, TurnScope? scope);
}