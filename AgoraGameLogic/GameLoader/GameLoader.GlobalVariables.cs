using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Entities;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Control.GameLoader;

public partial class GameLoader
{
    public Result AddGlobalVariableToContext(Context context, KeyValuePairBuildData keyValuePairBuildData)
    {
        object? value;
        
        if (keyValuePairBuildData.Value.Type == JTokenType.Null)
        {
            // value is null
            value = null;
        }
        else if (keyValuePairBuildData.Value.Type == JTokenType.Integer)
        {
            // values is an int
            value = keyValuePairBuildData.Value.ToObject<int>();
        }
        else if (keyValuePairBuildData.Value.Type == JTokenType.String)
        {
            // value is a string
            value = keyValuePairBuildData.Value.ToObject<string>();
        }
        else if (keyValuePairBuildData.Value.Type == JTokenType.Array && keyValuePairBuildData.Value.First != null)
        {
            // value is an array.  Check if it's an array of integers or strings
            var firstItemType = keyValuePairBuildData.Value.First.Type;
            if (firstItemType == JTokenType.Integer)
            {
                // value is int[]
                value = keyValuePairBuildData.Value.ToObject<int[]>();
            }
            else if (firstItemType == JTokenType.String)
            {
                // value is string[]
                value = keyValuePairBuildData.Value.ToObject<string[]>();
            }
            else
            {
                return Result.Failure("Unsupported array element type in 'Value'");
            }
        }
        else
        {
            return Result.Failure("Unsupported type in 'Value'");
        }

        var valueObject = Value<object?>.FromOrThrow(value);
        context.AddOrUpdate(keyValuePairBuildData.Key, ref valueObject);
        
        return Result.Success();
    }

    public Result AddBuiltInGlobalVariableToContext(Context context, IEnumerable<GameModule> players)
    {
        try
        {
            // players
            var playerList = players.ToList();
            context.AddOrUpdate("Players", ref playerList);
            
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
        
    }
}