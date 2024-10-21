using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Interfaces.Services;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Services;

public class InputService : CommandService<InputCommand>, IInputService
{
    public async Task<Result> ResolveInputAsync(IContext context, string playerName, int id, object? answer)
    {
        try
        {
            var commandResult = GetCommand(playerName, id);
            if (!commandResult.IsSuccess)
            {
                return Result.Failure(commandResult.Error);
            }
        
            var removeResult = RemoveCommand(id);
            if (!removeResult.IsSuccess)
            {
                return Result.Failure(removeResult.Error);
            }
        
            var performResult = await commandResult.Value.ResolveAsync(answer);
            return performResult.IsSuccess ? Result.Success() : Result.Failure(performResult.Error);
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    public bool HasUnresolvedInputs(GameModule player)
    {
        var store = CommandStoresByPlayerName[player.Id].GetAllCommands();
        if (!store.IsSuccess)
        {
            throw new Exception($"cannot get all commands for {player.Id}");
        }

        return store.Value.ToList().Exists(c => c.IsPriority);
    }

    public async Task<Result> ResolveNextInput(GameModule player)
    {
        var store = CommandStoresByPlayerName[player.Id].GetAllCommands();
        if (!store.IsSuccess)
        {
            throw new Exception($"cannot get all commands for {player.Id}");
        }
        
        var nextCommand =  store.Value.ToList().FirstOrDefault(c => c.IsPriority);
        if (nextCommand != null)
        {
            return await nextCommand.ResolveDefaultAsync();
        }

        return Result.Success();
    }
}