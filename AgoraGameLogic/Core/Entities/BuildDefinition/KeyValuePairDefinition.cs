using AgoraGameLogic.Domain.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Domain.Entities.BuildDefinition;

public class KeyValuePairDefinition
{
    public string Key { get; set; }
    public JToken Value { get; set; }
    
    public static KeyValuePairDefinition[] ParseArray(JArray jArray)
    {
        var result = new List<KeyValuePairDefinition>();
        foreach (var jToken in jArray)
        {
            result.Add(Parse(jToken));
        }

        return result.ToArray();
    }
    
    public static KeyValuePairDefinition Parse(JToken token)
    {
        var result = new KeyValuePairDefinition();
        try
        {
            result.Key = token.GetValueOrThrow<string>("Key");
            result.Value = token.GetValueOrThrow("Value");
        }
        catch (JsonException ex)  // Handle issues with deserialization
        {
            throw new InvalidOperationException("Failed to parse the token into a BlockDefinition.", ex);
        }

        return result;
    }
}