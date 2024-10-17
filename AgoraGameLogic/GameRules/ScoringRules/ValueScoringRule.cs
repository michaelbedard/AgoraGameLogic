using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Rules.ScoringRules;

public class ValueScoringRule : ScoringRule
{
    private Value<int> _value;
    
    public ValueScoringRule(ScoringRuleBuildData buildData, GameData gameData) : base(gameData)
    {
        _value =Value<int>.ParseOrThrow(buildData.Inputs[0], gameData);
    }

    public override Result<int> ResolveScore(IContext context, GameModule player)
    {
        return _value.GetValue(context);
    }
}