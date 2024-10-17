using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Rules.ScoringRules;

namespace AgoraGameLogic.Logic.Rules;

public class ScoringRuleFactory
{
    public static Result<ScoringRule> Create(ScoringRuleBuildData scoringRuleBuildData, GameData gameData)
    {
        ScoringRule? scoringRule;
        try
        {
            scoringRule = scoringRuleBuildData.Type switch
            {
                nameof(ForeachScoringRule) => new ForeachScoringRule(scoringRuleBuildData, gameData),
                nameof(ValueScoringRule) => new ValueScoringRule(scoringRuleBuildData, gameData),

                _ => null
            };

            if (scoringRule == null)
            {
                return Result<ScoringRule>.Failure($"Don't know how to create rule of type '{scoringRuleBuildData.Type}'");
            }
            
            return Result<ScoringRule>.Success(scoringRule);
        }
        catch (Exception e)
        {
            return Result<ScoringRule>.Failure(e.Message);
        }
    }
}