using System.Collections.Generic;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Dtos;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.GameLoader;

public partial class GameLoader
{
    private GameLoadDataDto GetLoadDataDto(Dictionary<GameModule, JToken> gameModuleToJToken, string name, int id, int numberOfPlayers)
    {
        
        // TODO

        return new GameLoadDataDto();
    }
}