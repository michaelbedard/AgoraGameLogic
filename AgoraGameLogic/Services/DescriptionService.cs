using System.Text;
using AgoraGameLogic.Control.GameLoader;
using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Logic;
using AgoraGameLogic.Logic.Blocks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Control.Services;

public class DescriptionService
{
    private Dictionary<GameModule,  List<object>> _description = new Dictionary<GameModule,  List<object>>();
    private List<GameModule> _players = new List<GameModule>();

    public void SetPlayers(IEnumerable<GameModule> players)
    {
        _players = players.ToList();
    }
    
    public void RegisterDescription(GameModule gameModule, JArray descriptionJArray, GameData gameData)
    {
        var segment = new List<object>();
        foreach (var descriptionJToken in descriptionJArray)
        {
            if (descriptionJToken.Type == JTokenType.String)
            {
                segment.Add(descriptionJToken.ToString());
            } 
            else
            {
                try
                {
                    if (descriptionJToken.HasValues)
                    {
                        var valueBlock = BlockFactory.Create<ValueBlockBase>(descriptionJToken, gameData);
                        segment.Add(valueBlock);
                    }
                }
                catch (JsonReaderException ex)
                {
                    Console.WriteLine($"Failed to parse logic block: {ex.Message}");
                }
            }
        }

        // Save the parsed description
        _description[gameModule] = segment;
    }
    
    public Dictionary<string, DescriptionDto[]> GetDescriptionDtos(IContext globalContext)
    {
        // iterate over each player
        var result = new Dictionary<string, DescriptionDto[]>();
        for (var i = 0; i < _players.Count(); i++)
        {
            var player = _players[i];
            var contextCopy = globalContext.Copy();
            contextCopy.AddOrUpdate("p", ref player);
            
            // build description for each gm w.r.t the player
            var resolvedDescriptions = new List<DescriptionDto>();
            foreach (var entry in _description)
            {
                var gameModule = entry.Key;
                contextCopy.AddOrUpdate("this", ref gameModule);
                
                var resolvedDescription = new DescriptionDto()
                {
                    Name = entry.Key.Name,
                    Text = ResolveDescription(entry.Value, contextCopy)
                };
            
                resolvedDescriptions.Add(resolvedDescription);
            }

            result[player.Name] = resolvedDescriptions.ToArray();
        }

        return result;
    }
    
    private string ResolveDescription(List<object> segments, IContext context)
    {
        var resolvedDescription = new StringBuilder();
        foreach (var segment in segments)
        {
            if (segment is string textSegment)
            {
                resolvedDescription.Append(textSegment);
            }
            else if (segment is ValueBlockBase valueBlock)
            {
                // Resolve the logic block (using provided function to resolve the block)
                var value = valueBlock.GetValue<object>(context);
                resolvedDescription.Append(JsonConvert.SerializeObject(value));
            }
            else
            {
                throw new Exception("segment is not supported");
            }
        }

        return resolvedDescription.ToString();
    }

}