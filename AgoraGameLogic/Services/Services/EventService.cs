using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Logic.Blocks;
using AgoraGameLogic.Logic.Blocks._options;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Control.Services;


public class EventService
{
    private Dictionary<GameModule, EventStore> _eventStoreByModule = new Dictionary<GameModule, EventStore>();
    private EventStore _globalEventStore = new EventStore();

    public void RegisterModuleEvent(GameModule gameModule, BaseEventBlock eventBlock)
    {
        if (!_eventStoreByModule.ContainsKey(gameModule))
        {
            _eventStoreByModule[gameModule] = new EventStore();
        }
        
        _eventStoreByModule[gameModule].AddEvent(eventBlock.GetType(), eventBlock);
    }
    
    public void RegisterGlobalEvent(BaseEventBlock eventBlock)
    {
        _globalEventStore.AddEvent(eventBlock.GetType(), eventBlock);
    }

    public async void TriggerEvents<T>(Context context, object[] args, Scope? scope) where T : BaseEventBlock
    {
        await TriggerEventsAsync<T>(context, args, scope);
    }

    public async Task TriggerEventsAsync<T>(Context context, object[] args, Scope? scope) where T : BaseEventBlock
    {
        // trigger game module events
        foreach (var entry in _eventStoreByModule)
        {
            // trigger the events
            foreach (var eventBlock in entry.Value.GetEvents(typeof(T)))
            {
                // OnlyTriggerIfTargetedBlock
                if (eventBlock.HasOption<OnlyTriggerIfTargetedBlock>()) return;

                var gameModule = entry.Key;
                context.AddOrUpdate("this", ref gameModule);
                await eventBlock.TriggerAsync(entry.Key, context, args, scope);
            }
        }

        // trigger global events
        foreach (var eventBlock in _globalEventStore.GetEvents(typeof(T)))
        {
            // OnlyTriggerIfTargetedBlock
            if (eventBlock.HasOption<OnlyTriggerIfTargetedBlock>()) return;
            
            await eventBlock.TriggerAsync(null, context, args, scope);
        }
    }
}

public class EventStore
{
    private readonly Dictionary<Type, List<BaseEventBlock>> _eventsByType;

    public EventStore()
    {
        _eventsByType = new Dictionary<Type, List<BaseEventBlock>>();
    }

    public void AddEvent(Type eventType, BaseEventBlock eventBlock)
    {
        if (!_eventsByType.ContainsKey(eventType))
        {
            _eventsByType[eventType] = new List<BaseEventBlock>();
        }

        _eventsByType[eventType].Add(eventBlock);
    }

    public List<BaseEventBlock> GetEvents(Type eventType)
    {
        if (_eventsByType.TryGetValue(eventType, out var eventBlocks))
        {
            return eventBlocks;
        }

        return new List<BaseEventBlock>();
    }
}