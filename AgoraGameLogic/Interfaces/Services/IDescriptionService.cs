using System.Collections.Generic;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Dtos;
using AgoraGameLogic.Interfaces.Actors;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Interfaces.Services;

public interface IDescriptionService
{
    Result RegisterDescriptionJArray(GameModule gameModule, JArray descriptionJArray, GameData gameData);
    Result<Dictionary<string, DescriptionDto[]>> GetDescriptionDtos(IContext globalContext);
    Result<string> ResolveDescription(List<object> segments, IContext context);
}