using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Interfaces.Services;

public interface IInputService : ICommandService<InputCommand>
{
    public bool HasUnresolvedInputs(GameModule player);
    public Task<Result> ResolveNextInput(GameModule player);
}