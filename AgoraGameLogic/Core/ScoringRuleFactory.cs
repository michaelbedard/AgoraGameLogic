using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Logic.Rules.ScoringRules;

namespace AgoraGameLogic.Logic.Rules;

public class ScoringRuleFactory
{
    public static ScoringRule Create(ScoringRuleDefinition scoringRuleDefinition, GameData gameData)
    {
        ScoringRule scoringRule;
        scoringRule = scoringRuleDefinition.Type switch
        {
            nameof(ForeachScoringRule) => new ForeachScoringRule(scoringRuleDefinition, gameData),
            nameof(ValueScoringRule) => new ValueScoringRule(scoringRuleDefinition, gameData),
            
            _ => throw new Exception($"Don't know how to create rule of type '{scoringRuleDefinition.Type}'")
        };
        
        return scoringRule;
    }
}