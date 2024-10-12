using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Rules.ScoringRules;

public class ValueScoringRule : ScoringRule
{
    private Value<int> _value;
    
    public ValueScoringRule(ScoringRuleDefinition definition, GameData gameData) : base(gameData)
    {
        _value =Value<int>.Parse(definition.Inputs[0], gameData);
    }

    public override int ResolveScore(IContext context, GameModule player)
    {
        return _value.GetValue(context);
    }
}