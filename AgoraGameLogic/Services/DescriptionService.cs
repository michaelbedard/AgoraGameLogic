using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Dtos;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Interfaces.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Services;

public class DescriptionService : IDescriptionService
{
    private Dictionary<GameModule,  List<object>> _description = new Dictionary<GameModule,  List<object>>();
    private List<GameModule> _players = new List<GameModule>();

    public void SetPlayers(IEnumerable<GameModule> players)
    {
        _players = players.ToList();
    }
    
    public Result RegisterDescriptionJArray(GameModule gameModule, JArray descriptionJArray, GameData gameData)
    {
        try
        {
            var segment = new List<object>();

            // iterate over all jToken inside jArray
            foreach (var descriptionJToken in descriptionJArray)
            {
                if (descriptionJToken.Type == JTokenType.String)
                {
                    // append string part (constant)
                    segment.Add(descriptionJToken.ToString());
                }
                else if (descriptionJToken.HasValues)
                {
                    // append block part (variable)
                    var valueBlockResult = BlockFactory.Create<ValueBlockBase>(descriptionJToken, gameData);
                    if (!valueBlockResult.IsSuccess)
                    {
                        return Result.Failure(valueBlockResult.Error);
                    }

                    segment.Add(valueBlockResult.Value);
                }
            }

            // Save the parsed description
            _description[gameModule] = segment;

            return Result.Success();
        }
        catch (JsonReaderException ex)
        {
            return Result.Failure($"Failed to parse logic block: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Unexpected error: {ex.Message}");
        }
    }

    public Result<Dictionary<string, DescriptionDto[]>> GetDescriptionDtos(IContext globalContext)
    {
        try
        {
            var result = new Dictionary<string, DescriptionDto[]>();

            // Iterate over each player
            for (var i = 0; i < _players.Count(); i++)
            {
                var player = _players[i];
                var contextCopy = globalContext.Copy();
                contextCopy.AddOrUpdate("p", ref player);

                // Build description for each game module with respect to the player
                var resolvedDescriptions = new List<DescriptionDto>();
                foreach (var entry in _description)
                {
                    // add 'this' to context
                    var gameModule = entry.Key;
                    contextCopy.AddOrUpdate("this", ref gameModule);

                    // resolve description
                    var resolvedTextResult = ResolveDescription(entry.Value, contextCopy);
                    if (!resolvedTextResult.IsSuccess)
                    {
                        return Result<Dictionary<string, DescriptionDto[]>>.Failure(resolvedTextResult.Error);
                    }

                    // build description dto
                    var resolvedDescription = new DescriptionDto
                    {
                        Name = gameModule.Name,
                        Text = resolvedTextResult.Value
                    };

                    resolvedDescriptions.Add(resolvedDescription);
                }

                // update result with descriptions
                result[player.Name] = resolvedDescriptions.ToArray();
            }

            return Result<Dictionary<string, DescriptionDto[]>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<Dictionary<string, DescriptionDto[]>>.Failure($"Unexpected error: {ex.Message}");
        }
    }

    
    public Result<string> ResolveDescription(List<object> segments, IContext context)
    {
        try
        {
            var resolvedDescription = new StringBuilder();

            // Build description by adding each part together
            foreach (var segment in segments)
            {
                if (segment is string textSegment)
                {
                    // Append string segment
                    resolvedDescription.Append(textSegment);
                }
                else if (segment is ValueBlockBase valueBlock)
                {
                    // Resolve the value block
                    var value = valueBlock.GetValue<object>(context);
                    resolvedDescription.Append(JsonConvert.SerializeObject(value));
                }
                else
                {
                    return Result<string>.Failure("Segment type is not supported.");
                }
            }

            // Return success with the built string
            return Result<string>.Success(resolvedDescription.ToString());
        }
        catch (Exception ex)
        {
            return Result<string>.Failure($"Error resolving description: {ex.Message}");
        }
    }
}