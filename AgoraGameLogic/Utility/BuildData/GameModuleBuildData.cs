using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Domain.Entities.BuildDefinition;

public class GameModuleBuildData
{
    public string Name;
    public GameModuleType Type;
    public string Structure;
    public KeyValuePairBuildData[] Fields;
    public BlockBuildData[] Blocks;
    
    // player specific
    public GameModuleBuildData[] Modules;
    
    // deck specific
    public GameModuleBuildData[] Cards;
    
    // common
    public JArray Description;
    public string Color { get; set; }
    public int[] Position { get; set; }
    public int Iterations { get; set; }
    public string FrontImage { get; set; }
    public string BackImage { get; set; }

    public static GameModuleBuildData[] ParseArrayOrThrow(JArray jArray)
    {
        var result = new List<GameModuleBuildData>();
        foreach (var jToken in jArray)
        {
            var buildDataResult = Parse(jToken);
            if (!buildDataResult.IsSuccess)
            {
                throw new Exception(buildDataResult.Error);
            }

            result.Add(buildDataResult.Value);
        }

        return result.ToArray();
    }

    public static Result<GameModuleBuildData> Parse(JToken token)
    {
        var result = new GameModuleBuildData();
        try
        {
            result.Name = token.GetItemOrThrow<string>("Name");
            result.Type = token.GetItemOrThrow<GameModuleType>("Type");
            result.Structure = token.GetItemOrThrow<string>("Structure");
            result.Fields = KeyValuePairBuildData.ParseArrayOrThrow(token.GetItemOrThrow("Fields").AsValidArray());
            result.Blocks = BlockBuildData.ParseArrayOrThrow(token.GetItemOrThrow("Blocks").AsValidArray());

            // Modules and Cards parsing
            result.Modules = ParseArrayOrThrow(token.GetItemOrDefault("Modules", new JArray()));
            result.Cards = ParseArrayOrThrow(token.GetItemOrDefault("Cards", new JArray()));

            // Common properties
            result.Description = token.GetItemOrDefault("Description", new JArray()).AsValidArray();
            result.Color = token.GetItemOrDefault<string>("Color", "");
            result.Position = token.GetItemOrDefault<int[]>("Position", Array.Empty<int>());
            result.Iterations = token.GetItemOrDefault<int>("Iterations", 1);
            result.FrontImage = token.GetItemOrDefault<string>("FrontImage", "");
            result.BackImage = token.GetItemOrDefault<string>("BackImage", "");

            return Result<GameModuleBuildData>.Success(result);
        }
        catch (JsonException ex)
        {
            return Result<GameModuleBuildData>.Failure($"JSON parsing error: {ex.Message}. Token: {token}");
        }
        catch (InvalidCastException ex)
        {
            return Result<GameModuleBuildData>.Failure($"Invalid type cast: {ex.Message}. Token: {token}");
        }
        catch (ArgumentException ex)
        {
            return Result<GameModuleBuildData>.Failure($"Invalid argument: {ex.Message}. Token: {token}");
        }
        catch (Exception ex)
        {
            return Result<GameModuleBuildData>.Failure($"Unexpected error: {ex.Message}. Token: {token}");
        }
    }
}