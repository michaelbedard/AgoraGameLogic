using AgoraGameLogic.Blocks;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Actors;

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