using AgoraGameLogic.Domain.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Domain.Entities.BuildDefinition;

public class BlockDefinition
{
    public string Type { get; set; }
    public BlockDefinition[] Options { get; set; }
    public JArray Inputs { get; set; }

    public static BlockDefinition[] ParseArray(JArray jArray)
    {
        var result = new List<BlockDefinition>();
        foreach (var jToken in jArray)
        {
            result.Add(Parse(jToken));
        }

        return result.ToArray();
    }
    
    public static BlockDefinition Parse(JToken token)
    {
        var result = new BlockDefinition();
        try
        {
            result.Type = token.GetValueOrThrow<string>("Type");
            result.Options = ParseArray(token.GetValueOrDefault("Options", new JArray()));
            result.Inputs = token.GetValueOrThrow("Inputs").AsValidArray();
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to parse the token into a BlockDefinition.", ex);
        }

        return result;
    }
}