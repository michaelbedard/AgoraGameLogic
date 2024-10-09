using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Control.GameLoader;

public partial class GameLoader
{
    private ClientLoadDto GetClientLoadData(Dictionary<GameModule, JToken> gameModuleToJToken, string name, int id, int numberOfPlayers)
    {
        
        // TODO

        return new ClientLoadDto();
    }
}