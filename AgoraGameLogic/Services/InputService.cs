using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Control.Services;

public class InputService : CommandService<InputCommand>, IInputService
{
    public async Task<Result> PerformInput(IContext context, string playerName, int id, object? answer)
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
        
            var performResult = await commandResult.Value.PerformAsync(context, answer, true);
            return performResult.IsSuccess ? Result.Success() : Result.Failure(performResult.Error);
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}