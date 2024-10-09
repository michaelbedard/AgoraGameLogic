using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;

namespace AgoraGameLogic.Control.Services;

public class ActionService : CommandService<BaseActionCommand>
{
    public async void PerformAction(Context context, string playerName, int id)
    {
        var command = GetCommand(playerName, id);
        
        RemoveCommand(id);
        await command.PerformAsync(context, true);
    }
}