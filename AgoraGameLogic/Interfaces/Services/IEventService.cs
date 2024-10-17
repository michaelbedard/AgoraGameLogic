using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Domain.Interfaces;

public interface IEventService
{
    Result RegisterModuleEvent(GameModule gameModule, EventBlockBase eventBlock);
    Result RegisterGlobalEvent(EventBlockBase eventBlock);
    Task<Result> TriggerEventsAsync<T>(IContext context, Command command, Scope? scope) where T : EventBlockBase;
}