using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Extensions;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Control.GameLoader;

public partial class GameLoader
{
    public void AddGlobalVariableToContext(Context context, KeyValuePairDefinition keyValuePairDefinition)
    {
        object? value;
        
        if (keyValuePairDefinition.Value.Type == JTokenType.Null)
        {
            value = null;
        }
        else if (keyValuePairDefinition.Value.Type == JTokenType.Integer)
        {
            value = keyValuePairDefinition.Value.ToObject<int>();
        }
        else if (keyValuePairDefinition.Value.Type == JTokenType.String)
        {
            value = keyValuePairDefinition.Value.ToObject<string>();
        }
        else if (keyValuePairDefinition.Value.Type == JTokenType.Array && keyValuePairDefinition.Value.First != null)
        {
            var firstItemType = keyValuePairDefinition.Value.First.Type;

            // Check if it's an array of integers or strings
            if (firstItemType == JTokenType.Integer)
            {
                value = keyValuePairDefinition.Value.ToObject<int[]>();
            }
            else if (firstItemType == JTokenType.String)
            {
                value = keyValuePairDefinition.Value.ToObject<string[]>();
            }
            else
            {
                throw new InvalidOperationException("Unsupported array element type in 'Value'.");
            }
        }
        else
        {
            throw new InvalidOperationException("Unsupported type in 'Value'.");
        }

        var valueObject = Value<object?>.From(value);
        context.AddOrUpdate(keyValuePairDefinition.Key, ref valueObject);
    }

    public void AddBuiltInGlobalVariableToContext(Context context, IEnumerable<GameModule> players)
    {
        // players
        var playerList = players.ToList();
        context.AddOrUpdate("Players", ref playerList);
    }
}