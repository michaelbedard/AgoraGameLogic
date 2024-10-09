using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;

namespace AgoraGameLogic.Logic.Rules.ScoringRules;

public class ForeachScoringRule : ScoringRule
{
    private Value<int> _foreachAmount;
    private Value<string> _nameOrStructure;
    private Value<string> _location;
    
    public ForeachScoringRule(ScoringRuleDefinition definition, GameData gameData) : base(gameData)
    {
        _foreachAmount = Value<int>.Parse(definition.Inputs[0], gameData);
        _nameOrStructure = Value<string>.Parse(definition.Inputs[1], gameData);
        _location = Value<string>.Parse(definition.Inputs[2], gameData);
    }

    public override int ResolveScore(Context context, GameModule player)
    {
        var nameOrStructure = _nameOrStructure.GetValue(context);
        var location = _location.GetValue(context);

        var items = new List<GameModule>();
        if (location == "Hand")
        {
            foreach (var card in player.Fields.Get<List<GameModule>>("Hand"))
            {
                AddIfValid(items, card, nameOrStructure);
            }
        }
        else
        {
            foreach (var child in player.GetChildren())
            {
                if (location == "" || location == "Any" || child.Name == location)
                {
                    AddIfValid(items, child, nameOrStructure);
                }
            }
        }

        return _foreachAmount.GetValue(context) * items.Count;
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