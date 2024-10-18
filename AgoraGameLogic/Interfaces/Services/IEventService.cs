using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Interfaces.Services;

public interface IEventService
{
    Result RegisterModuleEvent(GameModule gameModule, EventBlockBase eventBlock);
    Result RegisterGlobalEvent(EventBlockBase eventBlock);
    Task<Result> TriggerEventsAsync<T>(IContext context, Command command, Scope? scope) where T : EventBlockBase;
}