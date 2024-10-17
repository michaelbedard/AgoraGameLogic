using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Rules.ScoringRules;

public class ForeachScoringRule : ScoringRule
{
    private Value<int> _foreachAmount;
    private Value<string> _nameOrStructure;
    private Value<string> _location;
    
    public ForeachScoringRule(ScoringRuleBuildData buildData, GameData gameData) : base(gameData)
    {
        _foreachAmount = Value<int>.ParseOrThrow(buildData.Inputs[0], gameData);
        _nameOrStructure = Value<string>.ParseOrThrow(buildData.Inputs[1], gameData);
        _location = Value<string>.ParseOrThrow(buildData.Inputs[2], gameData);
    }

    public override Result<int> ResolveScore(IContext context, GameModule player)
    {
        try
        {
            // get values
            var forEachAmount = _foreachAmount.GetValueOrThrow(context);
            var nameOrStructure = _nameOrStructure.GetValueOrThrow(context);
            var location = _location.GetValueOrThrow(context);


            // logic
            var items = new List<GameModule>();
            if (location == "Hand")
            {
                // for each card in hand
                foreach (var card in player.Fields.Get<List<GameModule>>("Hand"))
                {
                    AddIfValid(items, card, nameOrStructure);
                }
            }
            else
            {
                // for each card in location
                foreach (var child in player.GetChildren())
                {
                    if (location == "" || location == "Any" || child.Name == location)
                    {
                        AddIfValid(items, child, nameOrStructure);
                    }
                }
            }

            return Result<int>.Success(forEachAmount * items.Count);
        }
        catch (Exception e)
        {
            return Result<int>.Failure(e.Message);
        }
        
    }

    private void AddIfValid(List<GameModule> items, GameModule gameModule, string nameOrStructure)
    {
        // any
        if (nameOrStructure == "" || nameOrStructure == "Any")
        {
            items.Add(gameModule);
            return;
        }

        // add if valid
        if (gameModule.Name == nameOrStructure || gameModule.Structures.Contains(nameOrStructure))
        {
            items.Add(gameModule);
        }
    }
}