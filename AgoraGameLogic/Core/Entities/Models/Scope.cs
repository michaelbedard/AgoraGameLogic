using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Logic.Blocks;

namespace AgoraGameLogic.Domain.Entities.Models;

public class Scope
{
    public BaseTurnBlock TurnBlock;
    public ScopeType ScopeType;
    public string PlayerId;

    public Scope(BaseTurnBlock turnBlock, ScopeType scopeType, string playerId)
    {
        TurnBlock = turnBlock;
        ScopeType = scopeType;
        PlayerId = playerId;
    }
}