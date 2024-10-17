using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Rules;

public abstract class ScoringRule
{
    protected GameData GameData;
    public ScoringRule(GameData gameData)
    {
        GameData = gameData;
    }
    
    public abstract Result<int> ResolveScore(IContext context, GameModule player);
}