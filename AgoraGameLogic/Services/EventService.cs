using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Logic.Blocks;
using AgoraGameLogic.Logic.Blocks._options;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Control.Services;


public class EventService
{
    private Dictionary<GameModule, EventStore> _eventStoreByModule = new Dictionary<GameModule, EventStore>();
    private EventStore _globalEventStore = new EventStore();

    public void RegisterModuleEvent(GameModule gameModule, EventBlockBase eventBlock)
    {
        if (!_eventStoreByModule.ContainsKey(gameModule))
        {
            _eventStoreByModule[gameModule] = new EventStore();
        }
        
        _eventStoreByModule[gameModule].AddEvent(eventBlock.GetType(), eventBlock);
    }
    
    public void RegisterGlobalEvent(EventBlockBase eventBlock)
    {
        _globalEventStore.AddEvent(eventBlock.GetType(), eventBlock);
    }

    public async void TriggerEvents<T>(IContext context, object[] args, Scope? scope) where T : EventBlockBase
    {
        await TriggerEventsAsync<T>(context, args, scope);
    }

    public async Task TriggerEventsAsync<T>(IContext context, object[] args, Scope? scope) where T : EventBlockBase
    {
        // trigger game module events
        foreach (var entry in _eventStoreByModule)
        {
            // trigger the events
            foreach (var eventBlock in entry.Value.GetEvents(typeof(T)))
            {
                var gameModule = entry.Key;
                await eventBlock.TriggerAsync(gameModule, context, args, scope);
            }
        }

        // trigger global events
        foreach (var eventBlock in _globalEventStore.GetEvents(typeof(T)))
        {
            await eventBlock.TriggerAsync(null, context, args, scope);
        }
    }
}

public class EventStore
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