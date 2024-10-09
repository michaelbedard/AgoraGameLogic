using AgoraGameLogic.Domain.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Domain.Entities.BuildDefinition;

public class StructureDefinition
{
    public string Name;
    public string Extension;
    public KeyValuePairDefinition[] Fields;
    public BlockDefinition[] Blocks;
    public JArray Description;

    public static StructureDefinition[] ParseArray(JArray jArray)
    {
        var result = new List<StructureDefinition>();
        foreach (var jToken in jArray)
        {
            result.Add(Parse(jToken));
        }

        return result.ToArray();
    }
    
    public static StructureDefinition Parse(JToken token)
    {
        var result = new StructureDefinition();
        try
        {
            result.Name = token.GetValueOrThrow<string>("Name");
            result.Extension = token.GetValueOrThrow<string>("Extension");
            result.Fields = KeyValuePairDefinition.ParseArray(token.GetValueOrThrow("Fields").AsValidArray());
            result.Blocks = BlockDefinition.ParseArray(token.GetValueOrThrow("Blocks").AsValidArray());
            
            result.Description = token.GetValueOrDefault("Description", new JArray());
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to parse the token into a BlockDefinition.", ex);
        }

        return result;
    }
}