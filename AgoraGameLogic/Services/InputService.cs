using System;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Interfaces.Services;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Services;

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
    
    protected override Result OnCommandFiltered()
    {
        // TODO force an input
        
        return Result.Success();
    }
}