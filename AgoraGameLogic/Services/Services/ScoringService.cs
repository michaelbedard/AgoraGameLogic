using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Logic.Rules;

namespace AgoraGameLogic.Control.Services;

public class ScoringService
{
    private Dictionary<string, ScoringRuleStore> _scoringRuleStoreByTag = new Dictionary<string, ScoringRuleStore>();
    private Context? _globalContext;
    
    public void SetGlobalContext(Context context)
    {
        _globalContext = context;
    }
    
    public void RegisterRule(string tag, ScoringRule rule)
    {
        if (!_scoringRuleStoreByTag.ContainsKey(tag))
        {
            _scoringRuleStoreByTag[tag] = new ScoringRuleStore();
        }
        
        _scoringRuleStoreByTag[tag].AddRule(rule);
    }

    public int GetScore(GameModule player)
    {
        var score = 0;
        foreach (var tag in _scoringRuleStoreByTag.Keys)
        {
            score += GetScoreForTag(player, tag);
        }

        return score;
    }
    
    public int GetScoreForTag(GameModule player, string tag)
    {
        if (_globalContext == null)
        {
            throw new Exception("Global context not set yet inside scoring service");
        }
        
        if (_scoringRuleStoreByTag.TryGetValue(tag, out var scoringRuleStore))
        {
            var score = 0;
            foreach (var scoringRule in scoringRuleStore.GetRules())
            {
                var contextCopy = _globalContext.Copy();
                contextCopy.AddOrUpdate("Player", ref player);
                
                score += scoringRule.ResolveScore(contextCopy, player);
            }

            return score;
        }

        throw new Exception($"No rules for tag {tag}");
    }
    
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

