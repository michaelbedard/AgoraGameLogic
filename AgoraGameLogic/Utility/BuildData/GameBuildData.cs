using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Domain.Entities.BuildDefinition;

public class GameBuildData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public StructureBuildData[] Structures { get; set; }
    public GameModuleBuildData[] GameModules { get; set; }
    public KeyValuePairBuildData[] GlobalVariables { get; set; }
    public BlockBuildData[] GlobalBlocks { get; set; }
    public ScoringRuleBuildData[] ScoringRules { get; set; }
    
    public static Result<GameBuildData> Parse(JToken token)
    {
        var result = new GameBuildData();
        try
        {
            // parse token
            var id = token.GetItemOrThrow<int>("Id");
            var name = token.GetItemOrThrow<string>("Name");
            var structuresJArray = token.GetItemOrThrow("Structures").AsValidArray();
            var gameModulesJArray = token.GetItemOrThrow("GameModules").AsValidArray();
            var globalVariablesJArray = token.GetItemOrThrow("GlobalVariables").AsValidArray();
            var globalBlocksJArray = token.GetItemOrThrow("GlobalBlocks").AsValidArray();
            var scoringRulesJArray = token.GetItemOrThrow("ScoringRules").AsValidArray();

            // bind to result
            result.Id = id;
            result.Name = name;
            result.Structures = StructureBuildData.ParseArrayOrThrow(structuresJArray);
            result.GameModules = GameModuleBuildData.ParseArrayOrThrow(gameModulesJArray);
            result.GlobalVariables = KeyValuePairBuildData.ParseArrayOrThrow(globalVariablesJArray);
            result.GlobalBlocks = BlockBuildData.ParseArrayOrThrow(globalBlocksJArray);
            result.ScoringRules = ScoringRuleBuildData.ParseArrayOrThrow(scoringRulesJArray);
            
            return Result<GameBuildData>.Success(result);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to parse the token into a BlockDefinition.", ex);
        }
    }
}