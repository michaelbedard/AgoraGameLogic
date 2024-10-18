using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Services;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Services;

public class AnimationService : CommandService<AnimationCommand>, IAnimationService
{
    protected override Result OnCommandFiltered()
    {
        return Result.Success();
    }
}