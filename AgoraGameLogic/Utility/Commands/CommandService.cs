using AgoraGameLogic.Core.Entities.Utility;
using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Blocks;

namespace AgoraGameLogic.Domain.Entities.Models;

public abstract class CommandService<TCommand> where TCommand : Command
{
    protected Dictionary<string, ICommandStore<TCommand>> CommandStoresByPlayerName = new Dictionary<string, ICommandStore<TCommand>>();
    private int _counter = 0;
    
    public Result PushCommand(TCommand item, GameModule player)
    {
        try
        {
            // init store for player, if needed
            if (!CommandStoresByPlayerName.ContainsKey(player.Name))
            {
                CommandStoresByPlayerName[player.Name] = new CommandStore<TCommand>();
            }

            // set id and register command
            item.Id = _counter++;
            var store = CommandStoresByPlayerName[player.Name];
            var result = store.RegisterCommand(item);

            return result.IsSuccess ? Result.Success() : Result.Failure(result.Error);
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
    
    public Result PullCommand(TCommand item, GameModule player)
    {
        if (!CommandStoresByPlayerName.ContainsKey(player.Name))
        {
            return Result.Failure($"No command store found for player {player.Name}");
        }
        
        var store = CommandStoresByPlayerName[player.Name];
        var commandsToRemove = new List<TCommand>();

        // get all commands for this player
        var allCommandsResult = store.GetAllCommands();
        if (!allCommandsResult.IsSuccess)
        {
            return Result.Failure(allCommandsResult.Error);
        }
        
        // iterate though command and find similar ones
        foreach (var command in allCommandsResult.Value)
        {
            if (item.Equals(command))
            {
                commandsToRemove.Add(command);
            }
        }

        // delete those commands
        foreach (var commandToRemove in commandsToRemove)
        {
            if (commandToRemove != null)
            {
                store.RemoveCommand(commandToRemove);
            }
        }
        
        return Result.Success();
    }

    
    public Result RemoveCommand(int commandId)
    {
        // iterate over each store
        foreach (var store in CommandStoresByPlayerName.Values)
        {
            // get contains result
            var containsResult = store.ContainsCommand(commandId);
            if (!containsResult.IsSuccess)
            {
                return Result.Failure(containsResult.Error);
            }

            // check if store contains command
            if (containsResult.Value)
            {
                return store.RemoveCommand(commandId);
            }
        }

        return Result.Failure($"Command with ID {commandId} not found.");
    }

    
    public Result<TCommand> GetCommand(string playerName, int commandId)
    {
        // get store for player
        if (CommandStoresByPlayerName.TryGetValue(playerName, out var store))
        {
            return store.GetCommand(commandId);
        }

        return Result<TCommand>.Failure($"Player {playerName} has no command store.");
    }

    
    public Result FilterActions(TurnBlockBlockBase turnBlock, ScopeType scopeType, GameModule player)
    {
        try
        {
            // iterate over each store
            foreach (var store in CommandStoresByPlayerName.Values)
            {
                // get all commands result
                var allCommandsResult = store.GetAllCommands();
                if (!allCommandsResult.IsSuccess)
                {
                    return Result.Failure(allCommandsResult.Error);
                }
                
                // for each command, remove command if scope is similar
                foreach (var command in allCommandsResult.Value)
                {
                    var scope = command.Scope;
                    if (scope != null && 
                        scope.TurnBlock == turnBlock && 
                        scope.ScopeType == scopeType && 
                        scope.PlayerId == player.Name)
                    {
                        store.RemoveCommand(command);
                    }
                }
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
    
    public Result<Dictionary<string, CommandDto[]>> GetDtos()
    {
        try
        {
            var result = CommandStoresByPlayerName.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.GetAllCommands().Value
                    .Select(a => a.GetDto())
                    .ToArray()
            );

            return Result<Dictionary<string, CommandDto[]>>.Success(result);
        }
        catch (Exception e)
        {
            return Result<Dictionary<string, CommandDto[]>>.Failure(e.Message);
        }
    }
}