using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Entities;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Domain.Interfaces;

public interface IDescriptionService
{
    Result RegisterDescriptionJArray(GameModule gameModule, JArray descriptionJArray, GameData gameData);
    Result<Dictionary<string, DescriptionDto[]>> GetDescriptionDtos(IContext globalContext);
    Result<string> ResolveDescription(List<object> segments, IContext context);
}