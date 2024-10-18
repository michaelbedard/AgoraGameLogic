using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Interfaces.Services;

public interface IActionService : ICommandService<ActionCommand>
{
    Task<Result> PerformActionAsync(IContext context, string playerName, int id);
}