using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Dtos;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Interfaces.Blocks;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Interfaces.Services;

public interface IEventService
{
    void SetGlobalContext(IContext context);
    Result RegisterModuleEvent(GameModule gameModule, IEventBlock eventBlock);
    Result RegisterGlobalEvent(IEventBlock eventBlock);
    Task<Result> TriggerEventsAsync<T>(TurnScope? turnScope, Command command) where T : IEventBlock;
}