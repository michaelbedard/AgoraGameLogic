using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Dtos;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Interfaces.Services;

public interface IEventService
{
    void SetGlobalContext(IContext context);
    Result RegisterModuleEvent(GameModule gameModule, EventBlock eventBlock);
    Result RegisterGlobalEvent(EventBlock eventBlock);
    Task<Result> TriggerEventsAsync<T>(TurnScope turnScope, Command command) where T : EventBlock;
}