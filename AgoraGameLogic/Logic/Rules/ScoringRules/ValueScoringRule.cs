using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;

namespace AgoraGameLogic.Logic.Rules.ScoringRules;

public class ValueScoringRule : ScoringRule
{
    private Value<int> _value;
    
    public ValueScoringRule(ScoringRuleDefinition definition, GameData gameData) : base(gameData)
    {
        _value =Value<int>.Parse(definition.Inputs[0], gameData);
    }

    public override int ResolveScore(Context context, GameModule player)
    {
        return _value.GetValue(context);
    }
}