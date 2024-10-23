using System;
using System.Text;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Blocks.Dev;

public class LogBlock : StatementBlock
{
    private List<object> Segments { get; set; }

    public LogBlock(BlockBuildData buildData, GameData gameData) : base (buildData, gameData)
    {
        Segments = new List<object>();

        // iterate over all jToken inside jArray
        foreach (var descriptionJToken in buildData.Inputs)
        {
            if (descriptionJToken.Type == JTokenType.String)
            {
                // append string part (constant)
                Segments.Add(descriptionJToken.ToString());
            }
            else if (descriptionJToken.HasValues)
            {
                // append block part (variable)
                var valueBlock = BlockFactory.CreateOrThrow<ValueBlock>(descriptionJToken, gameData);
                Segments.Add(valueBlock);
            }
        }
    }

    protected override async Task<Result> ExecuteAsyncCore()
    {
        try
        {
            var resolvedDescription = new StringBuilder();

            // Build description by adding each part together
            foreach (var segment in Segments)
            {
                if (segment is string textSegment)
                {
                    // Append string segment
                    resolvedDescription.Append(textSegment);
                }
                else if (segment is ValueBlock valueBlock)
                {
                    // Resolve the value block
                    var valueResult = valueBlock.GetValue<object>(Context);
                    if (!valueResult.IsSuccess)
                    {
                        return Result.Failure(valueResult.Error);
                    }
                    
                    resolvedDescription.Append(valueResult.Value.ToString());
                }
                else
                {
                    return Result<string>.Failure("Segment type is not supported.");
                }
            }
            
            // log the result
            Console.WriteLine(resolvedDescription.ToString());

            // Return success with the built string
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result<string>.Failure($"Error resolving description: {ex.Message}");
        }
    }
}