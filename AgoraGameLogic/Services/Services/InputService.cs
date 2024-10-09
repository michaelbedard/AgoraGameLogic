using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;

namespace AgoraGameLogic.Control.Services;

public class InputService : CommandService<BaseInputCommand>
{
    public async void PerformInput(Context context, string playerName, int id, object? answer)
    {
        var command = GetCommand(playerName, id);
        
        RemoveCommand(id);
        await command.PerformAsync(context, answer, true);
    }
}