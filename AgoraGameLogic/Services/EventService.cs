using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Interfaces.Blocks;
using AgoraGameLogic.Interfaces.Services;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Services;


public class EventService : IEventService
{
    private Dictionary<GameModule, EventStore> _eventStoreByModule = new Dictionary<GameModule, EventStore>();
    private EventStore _globalEventStore = new EventStore();

    private IContext _globalContext;

    public void SetGlobalContext(IContext context)
    {
        _globalContext = context;
    }

    /// <summary>
    /// Registers an event block for a specific game module.
    /// </summary>
    public Result RegisterModuleEvent(GameModule gameModule, IEventBlock eventBlock)
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
            return Result.Failure($"Failed to register module event: {ex.Message}", new ErrorBuilder()
            {
                ClassName = nameof(EventService),
                MethodName = nameof(RegisterModuleEvent)
            });
        }
    }
    
    /// <summary>
    /// Registers a global event block.
    /// </summary>
    public Result RegisterGlobalEvent(IEventBlock eventBlock)
    {
        try
        {
            _globalEventStore.AddEvent(eventBlock.GetType(), eventBlock);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to register global event: {ex.Message}", new ErrorBuilder()
            {
                ClassName = nameof(EventService),
                MethodName = nameof(RegisterModuleEvent)
            });
        }
    }

    /// <summary>
    /// Triggers events asynchronously and returns a Result.
    /// </summary>
    public async Task<Result> TriggerEventsAsync<T>(TurnScope? scope, Command command) where T : IEventBlock
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
                    var result = await eventBlock.TriggerAsync(_globalContext.Copy(), scope, command, gameModule);
                    if (!result.IsSuccess)
                    {
                        return Result.Failure(result.Error);
                    }
                }
            }

            // Trigger global events
            var globalEvents = _globalEventStore.GetEvents(typeof(T));
            foreach (var eventBlock in globalEvents)
            {
                var result = await eventBlock.TriggerAsync(_globalContext.Copy(), scope, command, null);
                if (!result.IsSuccess)
                {
                    return Result.Failure(result.Error);
                }
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
    
    /// <summary>
    /// private class that stores the event blocks by type
    /// </summary>
    private class EventStore
    {
        private readonly Dictionary<Type, List<IEventBlock>> _eventsByType;

        public EventStore()
        {
            _eventsByType = new Dictionary<Type, List<IEventBlock>>();
        }

        public void AddEvent(Type eventType, IEventBlock eventBlock)
        {
            if (!_eventsByType.ContainsKey(eventType))
            {
                _eventsByType[eventType] = new List<IEventBlock>();
            }

            _eventsByType[eventType].Add(eventBlock);
        }

        public List<IEventBlock> GetEvents(Type eventType)
        {
            if (_eventsByType.TryGetValue(eventType, out var eventBlocks))
            {
                return eventBlocks;
            }

            return new List<IEventBlock>();
        }
    }
}