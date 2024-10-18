using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;

namespace AgoraGameLogic.Interfaces.Services;

public interface IScoringService
{
    void SetGlobalContext(IContext context); 
    Result RegisterRule(string tag, ScoringRule rule);
    Result<int> GetScoreForPlayer(GameModule player);
    Result<int> GetScoreForPlayerForTag(GameModule player, string tag);
}