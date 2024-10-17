using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Domain.Entities.BuildDefinition;

public class BlockBuildData
{
    public string Type { get; set; }
    public BlockBuildData[] Options { get; set; }
    public JArray Inputs { get; set; }

/// <summary>
    /// Parses a JArray into an array of BlockBuildData objects by extracting and validating 
    /// each JToken in the array.
    /// </summary>
    public static Result<BlockBuildData[]> ParseArray(JArray jArray)
    {
        var result = new List<BlockBuildData>();

        foreach (var jToken in jArray)
        {
            var parseResult = Parse(jToken);
            if (!parseResult.IsSuccess)
            {
                return Result<BlockBuildData[]>.Failure(parseResult.Error);
            }

            result.Add(parseResult.Value);
        }

        return Result<BlockBuildData[]>.Success(result.ToArray());
    }

    /// <summary>
    /// Parses a JToken into a BlockBuildData object, extracting and validating the necessary fields 'Type', 'Options', and 'Inputs'.
    /// </summary>
    public static Result<BlockBuildData> Parse(JToken token)
    {
        try
        {
            // Parse 'Type' field
            var typeResult = token.GetItem<string>("Type");
            if (!typeResult.IsSuccess)
            {
                return Result<BlockBuildData>.Failure(typeResult.Error);
            }
            var type = typeResult.Value;

            // Parse 'Inputs' field
            var inputsResult = token.GetItem<JArray>("Inputs");
            if (!inputsResult.IsSuccess)
            {
                return Result<BlockBuildData>.Failure(inputsResult.Error);
            }

            // Parse 'Options' field, default to empty array if missing
            var optionsJArray = token.GetItemOrDefault("Options", new JArray());

            // Parse options into valid block data
            var parsedOptions = ParseArray(optionsJArray);
            if (!parsedOptions.IsSuccess)
            {
                return Result<BlockBuildData>.Failure(parsedOptions.Error);
            }

            // Create BlockBuildData object
            var result = new BlockBuildData
            {
                Type = type,
                Inputs = inputsResult.Value.AsValidArray(),
                Options = parsedOptions.Value
            };

            return Result<BlockBuildData>.Success(result);
        }
        catch (JsonException ex)
        {
            return Result<BlockBuildData>.Failure(
                $"JSON parsing error: Failed to parse the token into a BlockBuildData object. Details: {ex.Message}"
            );
        }
        catch (Exception ex)
        {
            return Result<BlockBuildData>.Failure(
                $"Unexpected error occurred while parsing the token: {ex.Message}"
            );
        }
    }


    /// <summary>
    /// Parses an array of BlockBuildData objects or throws an exception if parsing fails.
    /// </summary>
    public static BlockBuildData[] ParseArrayOrThrow(JArray jArray)
    {
        var parseResult = ParseArray(jArray);
        if (!parseResult.IsSuccess)
        {
            throw new Exception(parseResult.Error);
        }

        return parseResult.Value;
    }
}