using AgoraGameLogic.Domain.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Domain.Entities.BuildDefinition;

public class BuildDefinition
{
    public int Id { get; set; }
    public string Name { get; set; }
    public StructureDefinition[] Structures { get; set; }
    public GameModuleDefinition[] GameModules { get; set; }
    public KeyValuePairDefinition[] GlobalVariables { get; set; }
    public BlockDefinition[] GlobalBlocks { get; set; }
    public ScoringRuleDefinition[] ScoringRules { get; set; }
    
    public static BuildDefinition Parse(JToken token)
    {
        var result = new BuildDefinition();
        try
        {
            result.Id = token.GetValueOrThrow<int>("Id");
            result.Name = token.GetValueOrThrow<string>("Name");
            result.Structures = StructureDefinition.ParseArray(token.GetValueOrThrow("Structures").AsValidArray());
            result.GameModules = GameModuleDefinition.ParseArray(token.GetValueOrThrow("GameModules").AsValidArray());
            result.GlobalVariables = KeyValuePairDefinition.ParseArray(token.GetValueOrThrow("GlobalVariables").AsValidArray());
            result.GlobalBlocks = BlockDefinition.ParseArray(token.GetValueOrThrow("GlobalBlocks").AsValidArray());
            result.ScoringRules = ScoringRuleDefinition.ParseArray(token.GetValueOrThrow("ScoringRules").AsValidArray());
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to parse the token into a BlockDefinition.", ex);
        }

        return result;
    }
}