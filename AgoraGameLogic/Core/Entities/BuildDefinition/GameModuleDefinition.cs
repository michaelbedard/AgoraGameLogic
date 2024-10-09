using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Domain.Entities.BuildDefinition;

public class GameModuleDefinition
{
    public string Name;
    public GameModuleType Type;
    public string Structure;
    public KeyValuePairDefinition[] Fields;
    public BlockDefinition[] Blocks;
    
    // player specific
    public GameModuleDefinition[] Modules;
    
    // deck specific
    public GameModuleDefinition[] Cards;
    
    // common
    public JArray Description;
    public string Color { get; set; }
    public int[] Position { get; set; }
    public int Iterations { get; set; }
    public string FrontImage { get; set; }
    public string BackImage { get; set; }

    public static GameModuleDefinition[] ParseArray(JArray jArray)
    {
        var result = new List<GameModuleDefinition>();
        foreach (var jToken in jArray)
        {
            result.Add(Parse(jToken));
        }

        return result.ToArray();
    }
    
    public static GameModuleDefinition Parse(JToken token)
    {
        var result = new GameModuleDefinition();
        try
        {
            result.Name = token.GetValueOrThrow<string>("Name");
            result.Type = token.GetValueOrThrow<GameModuleType>("Type");
            result.Structure = token.GetValueOrThrow<string>("Structure");
            result.Fields = KeyValuePairDefinition.ParseArray(token.GetValueOrThrow("Fields").AsValidArray());
            result.Blocks = BlockDefinition.ParseArray(token.GetValueOrThrow("Blocks").AsValidArray());

            // modules
            result.Modules = ParseArray(token.GetValueOrDefault("Modules", new JArray()));
            
            // cards
            result.Cards = ParseArray(token.GetValueOrDefault("Cards", new JArray()));
            
            // common
            result.Description = token.GetValueOrDefault("Description", new JArray());
            result.Color = token.GetValueOrDefault("Color", "");
            result.Position = token.GetValueOrDefault("Position", Array.Empty<int>());
            result.Iterations = token.GetValueOrDefault("Iterations", 1);
            result.FrontImage = token.GetValueOrDefault("FrontImage", "");
            result.BackImage = token.GetValueOrDefault("BackImage", "");
        }
        catch (JsonException ex)  // Handle issues with deserialization
        {
            throw new InvalidOperationException("Failed to parse the token into a BlockDefinition.", ex);
        }

        return result;
    }
}