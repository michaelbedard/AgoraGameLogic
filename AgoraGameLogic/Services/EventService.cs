using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Blocks;
using AgoraGameLogic.Logic.Blocks._options;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Control.Services;


public class EventService : IEventService
{
    private Dictionary<GameModule, EventStore> _eventStoreByModule = new Dictionary<GameModule, EventStore>();
    private EventStore _globalEventStore = new EventStore();

    /// <summary>
    /// Registers an event block for a specific game module.
    /// </summary>
    public Result RegisterModuleEvent(GameModule gameModule, EventBlockBase eventBlock)
    {
        try
        {
            if (!_eventStoreByModule.ContainsKey(gameModule))
            {
                _eventStoreByModule[gameModule] = new EventStore();
            }

            _eventStoreByModule[gameModule].AddEvent(eventBlock.GetType(), eventBlock);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to register module event: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Registers a global event block.
    /// </summary>
    public Result RegisterGlobalEvent(EventBlockBase eventBlock)
    {
        try
        {
            _globalEventStore.AddEvent(eventBlock.GetType(), eventBlock);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to register global event: {ex.Message}");
        }
    }

    /// <summary>
    /// Triggers events asynchronously and returns a Result.
    /// </summary>
    public async Task<Result> TriggerEventsAsync<T>(IContext context, Command command, Scope? scope) where T : EventBlockBase
    {
        try
        {
            // Trigger events for each game module
            foreach (var entry in _eventStoreByModule)
            {
                var gameModule = entry.Key;
                var events = entry.Value.GetEvents(typeof(T));

                foreach (var eventBlock in events)
                {
                    var result = await eventBlock.TriggerAsync(gameModule, context, command, scope);
                    if (!result.IsSuccess)
                    {
                        return Result.Failure($"Failed to trigger event: {result.Error}");
                    }
                }
            }

            // Trigger global events
            var globalEvents = _globalEventStore.GetEvents(typeof(T));
            foreach (var eventBlock in globalEvents)
            {
                var result = await eventBlock.TriggerAsync(null, context, command, scope);
                if (!result.IsSuccess)
                {
                    return Result.Failure($"Failed to trigger global event: {result.Error}");
                }
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Unexpected error while triggering events: {ex.Message}");
        }
    }
    
    /// <summary>
    /// private class that stores the event blocks by type
    /// </summary>
    private class EventStore
    {
        private readonly Dictionary<Type, List<EventBlockBase>> _eventsByType;

        public EventStore()
        {
            _eventsByType = new Dictionary<Type, List<EventBlockBase>>();
        }

        public void AddEvent(Type eventType, EventBlockBase eventBlock)
        {
            if (!_eventsByType.ContainsKey(eventType))
            {
                _eventsByType[eventType] = new List<EventBlockBase>();
            }

            _eventsByType[eventType].Add(eventBlock);
        }

        public List<EventBlockBase> GetEvents(Type eventType)
        {
            if (_eventsByType.TryGetValue(eventType, out var eventBlocks))
            {
                return eventBlocks;
            }

            return new List<EventBlockBase>();
        }
    }
}