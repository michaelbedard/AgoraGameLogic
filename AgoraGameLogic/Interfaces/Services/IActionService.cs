using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Domain.Interfaces;

public interface IActionService : ICommandService<ActionCommand>
{
    Task<Result> PerformActionAsync(IContext context, string playerName, int id);
}