using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Rules;

public abstract class ScoringRule
{
    protected GameData GameData;
    public ScoringRule(GameData gameData)
    {
        GameData = gameData;
    }
    
    public abstract int ResolveScore(IContext context, GameModule player);
}