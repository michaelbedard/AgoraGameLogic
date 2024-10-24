using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Interfaces.Blocks;

public interface IEventBlock
{
    Task<Result> TriggerAsync(IContext context, TurnScope scope, Command command, GameModule? gameModule);
}