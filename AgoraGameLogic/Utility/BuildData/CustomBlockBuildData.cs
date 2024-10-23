using AgoraGameLogic.Actors;
using AgoraGameLogic.Utility.Enums;
using AgoraGameLogic.Utility.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Utility.BuildData;

public class CustomBlockBuildData
{
    public string Type { get; set; }
    public BlockType BlockType { get; set; }
    public BlockBuildData[] Options { get; set; }
    public JArray Block { get; set; } // this is the block.  Similarly to definition, it is a decomposed JArray
    public BlockBuildData Definition { get; set; }
    
    /// <summary>
    /// Parses a JToken into a CustomBlockBuildData object, extracting and validating the necessary fields.
    /// </summary>
    public static Result<CustomBlockBuildData> Parse(JToken token)
    {
        try
        {
            var result = new CustomBlockBuildData();
                
            // Parse 'Type' field
            var typeResult = token.GetItem<string>("Type");
            if (!typeResult.IsSuccess)
            {
                return Result<CustomBlockBuildData>.Failure(typeResult.Error);
            }
            var type = typeResult.Value;

            // Parse 'BlockType' field
            var inputsResult = token.GetItem<JArray>("BlockType");
            if (!inputsResult.IsSuccess)
            {
                return Result<CustomBlockBuildData>.Failure(inputsResult.Error);
            }

            // Parse 'Options' field, default to empty array if missing.  Then parse to valid block data
            var optionsJArray = token.GetItemOrDefault("Options", new JArray());
            var parsedOptionsResult = BlockBuildData.ParseArray(optionsJArray);
            if (!parsedOptionsResult.IsSuccess)
            {
                return Result<CustomBlockBuildData>.Failure(parsedOptionsResult.Error);
            }
            
            // parse 'Block' field
            result.Block = token.GetItemOrDefault("Block", new JArray()).AsValidArray();
            
            // parse 'Definition' field
            var definitionJArray = token.GetItemOrDefault("Definition", new JArray());
            var parsedDefinitionResult = BlockBuildData.Parse(definitionJArray);
            if (!parsedDefinitionResult.IsSuccess)
            {
                return Result<CustomBlockBuildData>.Failure(parsedDefinitionResult.Error);
            }

            
            result.Options = parsedOptionsResult.Value;
            result.Definition = parsedDefinitionResult.Value;

            return Result<CustomBlockBuildData>.Success(result);
        }
        catch (JsonException ex)
        {
            return Result<CustomBlockBuildData>.Failure(
                $"JSON parsing error: Failed to parse the token into a CustomBlockBuildData object. Details: {ex.Message}"
            );
        }
        catch (Exception ex)
        {
            return Result<CustomBlockBuildData>.Failure(
                $"Unexpected error occurred while parsing the token: {ex.Message}"
            );
        }
    }
}