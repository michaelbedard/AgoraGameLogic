using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Domain.Entities.BuildDefinition;

public class KeyValuePairBuildData
{
    public string Key { get; set; }
    public JToken Value { get; set; }
    
    public static KeyValuePairBuildData[] ParseArrayOrThrow(JArray jArray)
    {
        var result = new List<KeyValuePairBuildData>();
        foreach (var jToken in jArray)
        {
            var jTokenResult = Parse(jToken);
            if (!jTokenResult.IsSuccess)
            {
                throw new Exception(jTokenResult.Error);
            }
            
            result.Add(jTokenResult.Value);
        }

        return result.ToArray();
    }
    
    public static Result<KeyValuePairBuildData> Parse(JToken token)
    {
        try
        {
            var keyValuePair = new KeyValuePairBuildData();
            
            keyValuePair.Key = token.GetItemOrThrow<string>("Key");
            keyValuePair.Value = token.GetItemOrThrow("Value");
            
            return Result<KeyValuePairBuildData>.Success(keyValuePair);
        }
        catch (Exception ex)
        {
            return Result<KeyValuePairBuildData>.Failure(ex.Message);
        }
    }
}