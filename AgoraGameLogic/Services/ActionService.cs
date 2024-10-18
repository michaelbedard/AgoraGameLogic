using System;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Interfaces.Services;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Services;

public class ActionService : CommandService<ActionCommand>, IActionService
{
    public async Task<Result> PerformActionAsync(IContext context, string playerId, int id)
    {
        try
        {
            var commandResult = GetCommand(playerId, id);
            if (!commandResult.IsSuccess)
            {
                return Result.Failure(commandResult.Error);
            }

            var removeResult = RemoveCommand(id);
            if (!removeResult.IsSuccess)
            {
                return Result.Failure(removeResult.Error);
            }

            return await commandResult.Value.PerformAsync(context, true);
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message, new ErrorBuilder()
            {
                ClassName = nameof(ActionService)
            });
        }
    }
}