using AgoraGameLogic.Domain.Entities.Models;

namespace AgoraGameLogic.Core.Entities.Utility;

public class CommandStore<T> where T : CommandBase
{
    private readonly Dictionary<int, T> _commandsById;
        
    public CommandStore()
    {
        _commandsById = new Dictionary<int, T>();
    }
        
    public void RegisterCommand(T command)
    {
        _commandsById[command.Id] = command;
    }

    public T? GetCommand(int id)
    {
        if (_commandsById.TryGetValue(id, out var command))
        {
            return command;
        }

        return null;
    }
        
    public IEnumerable<T> GetAllCommands()
    {
        return _commandsById.Values;
    }
    
    public bool ContainsCommand(int id)
    {
        return _commandsById.ContainsKey(id);
    }

    public void RemoveCommand(int id)
    {
        _commandsById.Remove(id);
    }
        
    public void RemoveCommand(T command)
    {
        RemoveCommand(command.Id);
    }
}