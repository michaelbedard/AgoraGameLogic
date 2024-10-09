using AgoraGameLogic.Domain.Entities.Models;

namespace AgoraGameLogic.Logic.Rules;

public abstract class ScoringRule
{
    protected GameData GameData;
    public ScoringRule(GameData gameData)
    {
        GameData = gameData;
    }
    
    public abstract int ResolveScore(Context context, GameModule player);
}