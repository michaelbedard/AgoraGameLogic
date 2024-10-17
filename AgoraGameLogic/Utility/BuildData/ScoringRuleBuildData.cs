using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Domain.Entities.BuildDefinition;

public class ScoringRuleBuildData
{
    public string Tag;
    public string Type;
    public JArray Inputs;
    
    public static ScoringRuleBuildData[] ParseArrayOrThrow(JArray jArray)
    {
        var result = new List<ScoringRuleBuildData>();
        foreach (var jToken in jArray)
        {
            var parseResult = Parse(jToken);
            if (!parseResult.IsSuccess)
            {
                throw new Exception(parseResult.Error);
            }
            result.Add(parseResult.Value);
        }

        return result.ToArray();
    }

    public static Result<ScoringRuleBuildData> Parse(JToken token)
    {
        try
        {
            var result = new ScoringRuleBuildData
            {
                Tag = token.GetItemOrThrow<string>("Tag"),
                Type = token.GetItemOrThrow<string>("Type"),
                Inputs = token.GetItemOrThrow("Inputs").AsValidArray()
            };
            return Result<ScoringRuleBuildData>.Success(result);
        }
        catch (JsonException ex)
        {
            return Result<ScoringRuleBuildData>.Failure($"Failed to parse ScoringRuleBuildData: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result<ScoringRuleBuildData>.Failure($"Unexpected error: {ex.Message}");
        }
    }
}