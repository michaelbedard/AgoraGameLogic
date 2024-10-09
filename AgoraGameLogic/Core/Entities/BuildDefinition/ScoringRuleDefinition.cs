using AgoraGameLogic.Domain.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Domain.Entities.BuildDefinition;

public class ScoringRuleDefinition
{
    public string Tag;
    public string Type;
    public JArray Inputs;
    
    public static ScoringRuleDefinition[] ParseArray(JArray jArray)
    {
        var result = new List<ScoringRuleDefinition>();
        foreach (var jToken in jArray)
        {
            result.Add(Parse(jToken));
        }

        return result.ToArray();
    }
    
    public static ScoringRuleDefinition Parse(JToken token)
    {
        var result = new ScoringRuleDefinition();
        try
        {
            result.Tag = token.GetValueOrThrow<string>("Tag");
            result.Type = token.GetValueOrThrow<string>("Type");
            result.Inputs = token.GetValueOrThrow("Inputs").AsValidArray();
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to parse the token into a ScoringRuleDefinition.", ex);
        }

        return result;
    }
}