using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Core.Entities.Utility;
using AgoraGameLogic.Dtos;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.Commands;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Services;

public abstract class CommandService<TCommand> where TCommand : Command
{
    protected IContext GlobalContext;
    
    protected Dictionary<string, ICommandStore<TCommand>> CommandStoresByPlayerName = new Dictionary<string, ICommandStore<TCommand>>();
    private int _counter = 0;

    public void SetGlobalContext(IContext context)
    {
        GlobalContext = context;
    }

    public Result PushCommand(TCommand item, GameModule player)
    {
        try
        {
            // set id and register command
            item.Id = _counter++;
            var store = CommandStoresByPlayerName[player.Id];
            var registerResult = store.RegisterCommand(item);

            return registerResult;
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message, new ErrorBuilder()
            {
                ClassName = nameof(CommandService<TCommand>),
                MethodName = nameof(PushCommand),
            });
        }
    }
    
    public Result PullCommand(TCommand item, GameModule player)
    {
        if (!CommandStoresByPlayerName.ContainsKey(player.Id))
        {
            return Result.Failure($"No command store found for player {player.Id}", new ErrorBuilder()
            {
                ClassName = nameof(CommandService<TCommand>),
                MethodName = nameof(PullCommand),
            });
        }
        
        var store = CommandStoresByPlayerName[player.Id];
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

        return Result.Failure($"Command with ID {commandId} not found.", new ErrorBuilder()
        {
            ClassName = nameof(CommandService<TCommand>),
            MethodName = nameof(RemoveCommand),
        });
    }

    
    public Result<TCommand> GetCommand(string playerId, int commandId)
    {
        // get store for player
        if (!CommandStoresByPlayerName.ContainsKey(playerId))
        {
            return Result<TCommand>.Failure($"Cannot get command {commandId} as player {playerId} has no command store yet", new ErrorBuilder()
            {
                ClassName = nameof(CommandService<TCommand>),
                MethodName= nameof(GetCommand)
            });
        }
        
        return CommandStoresByPlayerName[playerId].GetCommand(commandId);
    }

    
    public Result FilterCommands(TurnScope scope)
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
                    if (scope.Equals(command.Scope))
                    {
                        var removeResult = store.RemoveCommand(command);
                        if (!removeResult.IsSuccess)
                        {
                            return Result.Failure(removeResult.Error);
                        }
                    }
                }
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result<TCommand>.Failure(e.Message, new ErrorBuilder()
            {
                ClassName = nameof(CommandService<TCommand>),
                MethodName= nameof(GetCommand)
            });
        }
    }
    
    public Result<Dictionary<string, CommandDto[]>> GetDtos()
    {
        try
        {
            var resultDictionary = new Dictionary<string, CommandDto[]>();
            foreach (var commandStoreByPlayer in CommandStoresByPlayerName)
            {
                var allCommandsResult = commandStoreByPlayer.Value.GetAllCommands();
                if (!allCommandsResult.IsSuccess)
                {
                    return Result<Dictionary<string, CommandDto[]>>.Failure(allCommandsResult.Error);
                }
                
                var commandDtoList = new List<CommandDto>();
                foreach (var command in allCommandsResult.Value)
                {
                    commandDtoList.Add(command.GetDto());
                }

                resultDictionary[commandStoreByPlayer.Key] = commandDtoList.ToArray();
            }

            return Result<Dictionary<string, CommandDto[]>>.Success(resultDictionary);
        }
        catch (Exception e)
        {
            return Result<Dictionary<string, CommandDto[]>>.Failure(e.Message, new ErrorBuilder()
            {
                ClassName = nameof(CommandService<TCommand>),
                MethodName= nameof(GetCommand)
            });
        }
    }

    public Result InitializeDictionaryEntries(List<GameModule> players)
    {
        try
        {
            foreach (var player in players)
            {
                CommandStoresByPlayerName[player.Id] = new CommandStore<TCommand>();
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}