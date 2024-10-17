using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Rules;

namespace AgoraGameLogic.Control.Services;

public class ScoringService : IScoringService
{
    private Dictionary<string, ScoringRuleStore> _scoringRuleStoreByTag = new Dictionary<string, ScoringRuleStore>();
    private IContext? _globalContext;
    
    public void SetGlobalContext(IContext context)
    {
        _globalContext = context;
    }
    
    /// <summary>
    /// Registers a scoring rule under a specific tag.
    /// </summary>
    public Result RegisterRule(string tag, ScoringRule rule)
    {
        try
        {
            if (!_scoringRuleStoreByTag.ContainsKey(tag))
            {
                _scoringRuleStoreByTag[tag] = new ScoringRuleStore();
            }

            _scoringRuleStoreByTag[tag].AddRule(rule);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to register rule: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets the total score for a player across all tags.
    /// </summary>
    public Result<int> GetScoreForPlayer(GameModule player)
    {
        try
        {
            var totalScore = 0;

            // foreach tags
            foreach (var tag in _scoringRuleStoreByTag.Keys)
            {
                // get score
                var tagScoreResult = GetScoreForPlayerForTag(player, tag);
                if (!tagScoreResult.IsSuccess)
                {
                    return Result<int>.Failure(tagScoreResult.Error);
                }

                // add to total
                totalScore += tagScoreResult.Value;
            }

            return Result<int>.Success(totalScore);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure($"Error calculating score: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Gets the score for a player for a specific tag.
    /// </summary>
    public Result<int> GetScoreForPlayerForTag(GameModule player, string tag)
    {
        if (_globalContext == null)
        {
            return Result<int>.Failure("Global context is not set.");
        }

        if (!_scoringRuleStoreByTag.TryGetValue(tag, out var scoringRuleStore))
        {
            return Result<int>.Failure($"No rules found for tag '{tag}'.");
        }

        try
        {
            var score = 0;

            // for each registered rule
            foreach (var scoringRule in scoringRuleStore.GetRules())
            {
                // add 'player' to context
                var contextCopy = _globalContext.Copy();
                contextCopy.AddOrUpdate("Player", ref player);
                
                // resolve and add score
                var resolvedScoreResult = scoringRule.ResolveScore(contextCopy, player);
                if (!resolvedScoreResult.IsSuccess)
                {
                    return Result<int>.Failure(resolvedScoreResult.Error);
                }
                
                score += resolvedScoreResult.Value;
            }

            return Result<int>.Success(score);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure($"Error resolving score for tag '{tag}': {ex.Message}");
        }
    }
    
    /// <summary>
    /// A private class for managing scoring rules
    /// </summary>
    private class ScoringRuleStore
    {
        private readonly List<ScoringRule> _scoringRules;

        public ScoringRuleStore()
        {
            _scoringRules = new List<ScoringRule>();
        }

        public void AddRule(ScoringRule rule)
        {
            _scoringRules.Add(rule);
        }

        public List<ScoringRule> GetRules()
        {
            return _scoringRules;
        }
    }
}

