using AgoraGameLogic.Core.Entities.Utility;
using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Logic.Blocks;

namespace AgoraGameLogic.Domain.Entities.Models;

public abstract class CommandService
{
    
}

public abstract class CommandService<TCommand> : CommandService where TCommand : BaseCommand
{
    protected Dictionary<string, CommandStore<TCommand>> CommandStoresByPlayerName = new Dictionary<string, CommandStore<TCommand>>();
    private int _counter = 0;
    
    public void PushCommand(TCommand item, GameModule player)
    {
        if (!CommandStoresByPlayerName.ContainsKey(player.Name))
        {
            CommandStoresByPlayerName[player.Name] = new CommandStore<TCommand>();
        }

        item.Id = _counter;
        _counter++;
        
        CommandStoresByPlayerName[player.Name].RegisterCommand(item);
    }

    public void PullCommand(TCommand item, GameModule player)
    {
        if (!CommandStoresByPlayerName.ContainsKey(player.Name))
        {
            return;
        }
        
        TCommand? commandToRemove = null;
        var actionStore = CommandStoresByPlayerName[player.Name];
        foreach (var command in actionStore.GetAllCommands())
        {
            if (item.Equals(command))
            {
                commandToRemove = command;
                break;
            }
        }

        if (commandToRemove != null)
        {
            actionStore.RemoveCommand(commandToRemove);
        }
    }
    
    public void RemoveCommand(int commandId)
    {
        foreach (var commandStore in CommandStoresByPlayerName.Values)
        {
            if (commandStore.ContainsCommand(commandId))
            {
                commandStore.RemoveCommand(commandId);
                return;
            }
        }

        throw new Exception($"Command {commandId} not found");
    }
    
    public TCommand GetCommand(string playerName, int commandId)
    {
        if (CommandStoresByPlayerName.TryGetValue(playerName, out var commandStore))
        {
            var command = commandStore.GetCommand(commandId);
            
            if (command != null)
            {
                return command;
            }
        }
        
        throw new Exception($"Player {playerName} has no command with Id {commandId}");
    }
    
    public void FilterActions(BaseTurnBlock turnBlock, ScopeType scopeType, GameModule player)
    {
        foreach (var commandStore in CommandStoresByPlayerName.Values)
        {
            foreach (var command in commandStore.GetAllCommands())
            {
                var scope = command.Scope;
                if (scope == null) continue;

                if (scope.TurnBlock == turnBlock && scope.ScopeType == scopeType && scope.PlayerId == player.Name)
                {
                    commandStore.RemoveCommand(command);
                }
            }
        }
    }
    
    public Dictionary<string, BaseCommandDto[]> GetDtos()
    {
        var result = CommandStoresByPlayerName.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.GetAllCommands() // Assuming GetAllCommands() returns IEnumerable<TCommand>
                .Select(a => a.GetDto()) // Apply the provided function to transform TCommand into TDto
                .ToArray()
        );

        return result;
    }
}