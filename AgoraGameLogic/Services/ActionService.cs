using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Services;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Services;

public class ActionService : CommandService<ActionCommand>, IActionService
{
    public async Task<Result> PerformActionAsync(string playerId, int id)
    {
        try
        {
            // get the command
            var commandResult = GetCommand(playerId, id);
            if (!commandResult.IsSuccess)
            {
                return Result.Failure(commandResult.Error);
            }

            // remove the command
            var removeResult = RemoveCommand(id);
            if (!removeResult.IsSuccess)
            {
                return Result.Failure(removeResult.Error);
            }

            // perform command
            return await commandResult.Value.PerformAsync(true);
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message, new ErrorBuilder()
            {
                ClassName = nameof(ActionService)
            });
        }
    }

    public async Task<Result> ForcePerformActionAsync(string playerId)
    {
        var store = CommandStoresByPlayerId[playerId].GetAllCommands();
        if (!store.IsSuccess)
        {
            return Result.Failure($"cannot get all commands for {playerId}");
        }

        var allowedActions = store.Value.ToList();
        if (allowedActions.Count == 0)
        {
            return Result.Failure($"{playerId} do not have actions.  Cannot force");
        }

        return await allowedActions[0].PerformAsync(true);
    }
}