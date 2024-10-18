using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.Commands;

// ReSharper disable All

namespace AgoraGameLogic.Core.Entities.Utility;

public class CommandStore<T> : ICommandStore<T> where T : Command
{
    private readonly Dictionary<int, T> _commandsById;
        
    public CommandStore()
    {
        _commandsById = new Dictionary<int, T>();
    }
        
    public Result RegisterCommand(T command)
    {
        if (command == null)
            return Result.Failure("Command cannot be null.");
        
        try
        {
            _commandsById[command.Id] = command;
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    public Result<T> GetCommand(int id)
    {
        try
        {
            if (_commandsById.TryGetValue(id, out var command))
            {
                return Result<T>.Success(command);
            }
            else
            {
                return Result<T>.Failure($"No command found with ID {id}.");
            }
        }
        catch (Exception e)
        {
            return Result<T>.Failure(e.Message);
        }
    }

    public Result<IEnumerable<T>> GetAllCommands()
    {
        try
        {
            return Result<IEnumerable<T>>.Success(_commandsById.Values);
        }
        catch (Exception e)
        {
            return Result<IEnumerable<T>>.Failure(e.Message);
        }
    }
    
    public Result<bool> ContainsCommand(int id)
    {
        try
        {
            bool exists = _commandsById.ContainsKey(id);
            return Result<bool>.Success(exists);
        }
        catch (Exception e)
        {
            return Result<bool>.Failure(e.Message);
        }
    }


    public Result RemoveCommand(int id)
    {
        try
        {
            if (_commandsById.Remove(id))
                return Result.Success();
            else
                return Result.Failure($"No command found with ID {id}.");
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
        
    public Result RemoveCommand(T command)
    {
        if (command == null)
            return Result.Failure("Command cannot be null.");

        return RemoveCommand(command.Id);
    }
}