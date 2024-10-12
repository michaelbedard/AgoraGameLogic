using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Control.Services;

public class ActionService : CommandService<ActionCommandBase>
{
    public async void PerformAction(IContext context, string playerName, int id)
    {
        var command = GetCommand(playerName, id);
        
        RemoveCommand(id);
        await command.PerformAsync(context, true);
    }
}