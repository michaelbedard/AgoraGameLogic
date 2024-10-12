using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Control.Services;

public class InputService : CommandService<InputCommandBase>
{
    public async void PerformInput(IContext context, string playerName, int id, object? answer)
    {
        var command = GetCommand(playerName, id);
        
        RemoveCommand(id);
        await command.PerformAsync(context, answer, true);
    }
}