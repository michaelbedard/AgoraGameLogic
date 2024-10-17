using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Control.Services;

public class ActionService : CommandService<ActionCommand>, IActionService
{
    public async Task<Result> PerformActionAsync(IContext context, string playerName, int id)
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

            var performResult = await commandResult.Value.PerformAsync(context, true);
            return performResult.IsSuccess ? Result.Success() : Result.Failure(performResult.Error);
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}