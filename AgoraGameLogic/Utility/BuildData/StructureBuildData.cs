using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Domain.Entities.BuildDefinition;

public class StructureBuildData
{
    public string Name;
    public string Extension;
    public KeyValuePairBuildData[] Fields;
    public BlockBuildData[] Blocks;
    public JArray Description;

    public static StructureBuildData[] ParseArrayOrThrow(JArray jArray)
    {
        var result = new List<StructureBuildData>();
        foreach (var jToken in jArray)
        {
            var buildDataResult = Parse(jToken);
            if (!buildDataResult.IsSuccess)
            {
                throw new Exception($"Failed to parse StructureBuildData: {buildDataResult.Error}");
            }
            
            result.Add(buildDataResult.Value);
        }

        return result.ToArray();
    }
    
    public static Result<StructureBuildData> Parse(JToken token)
    {
        var result = new StructureBuildData();
        try
        {
            result.Name = token.GetItemOrThrow<string>("Name");
            result.Extension = token.GetItemOrThrow<string>("Extension");
            result.Fields = KeyValuePairBuildData.ParseArrayOrThrow(token.GetItemOrDefault("Fields", new JArray()).AsValidArray());
            result.Blocks = BlockBuildData.ParseArrayOrThrow(token.GetItemOrDefault("Blocks", new JArray()).AsValidArray());
            result.Description = token.GetItemOrDefault("Description", new JArray());
            
            return Result<StructureBuildData>.Success(result);
        }
        catch (JsonException ex)
        {
            return Result<StructureBuildData>.Failure($"JSON parsing error: {ex.Message}. Token: {token}");
        }
        catch (InvalidCastException ex)
        {
            return Result<StructureBuildData>.Failure($"Invalid type cast: {ex.Message}. Token: {token}");
        }
        catch (ArgumentException ex)
        {
            return Result<StructureBuildData>.Failure($"Invalid argument: {ex.Message}. Token: {token}");
        }
        catch (Exception ex)
        {
            return Result<StructureBuildData>.Failure($"Unexpected error: {ex.Message}. Token: {token}");
        }
    }
}