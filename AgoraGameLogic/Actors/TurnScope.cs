using AgoraGameLogic.Blocks;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Actors;

public class TurnScope : IEquatable<TurnScope>
{
    public TurnBlock TurnBlock { get; set; }
    public TurnState TurnState { get; set; }
    public GameModule Player { get; set; }


    public bool Equals(TurnScope? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return TurnBlock.Equals(other.TurnBlock) && TurnState == other.TurnState && Player.Equals(other.Player);
    }
}