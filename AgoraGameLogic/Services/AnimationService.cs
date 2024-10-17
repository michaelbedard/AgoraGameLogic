using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Control.Services;

public class AnimationService : CommandService<AnimationCommand>, IAnimationService
{
}