using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Interfaces.Services;

public interface IActionService : ICommandService<ActionCommand>
{
    Task<Result> PerformActionAsync(string playerId, int id);
    Task<Result> ForcePerformActionAsync(string playerId);
}