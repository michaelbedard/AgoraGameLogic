using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Rules;

namespace AgoraGameLogic.Domain.Interfaces;

public interface IScoringService
{
    void SetGlobalContext(IContext context); 
    Result RegisterRule(string tag, ScoringRule rule);
    Result<int> GetScoreForPlayer(GameModule player);
    Result<int> GetScoreForPlayerForTag(GameModule player, string tag);
}