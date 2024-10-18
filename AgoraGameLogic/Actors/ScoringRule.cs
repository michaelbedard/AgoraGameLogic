using AgoraGameLogic.Interfaces.Actors;

namespace AgoraGameLogic.Actors;

public abstract class ScoringRule
{
    protected GameData GameData;
    public ScoringRule(GameData gameData)
    {
        GameData = gameData;
    }
    
    public abstract Result<int> ResolveScore(IContext context, GameModule player);
}