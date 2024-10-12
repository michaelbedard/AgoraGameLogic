using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Logic.Blocks;

namespace AgoraGameLogic.Domain.Entities.Models;

public class Scope
{
    public TurnBlockBlockBase TurnBlock;
    public ScopeType ScopeType;
    public string PlayerId;

    public Scope(TurnBlockBlockBase turnBlock, ScopeType scopeType, string playerId)
    {
        TurnBlock = turnBlock;
        ScopeType = scopeType;
        PlayerId = playerId;
    }
}