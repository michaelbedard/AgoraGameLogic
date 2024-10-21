using AgoraGameLogic.Blocks;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Actors;

public class TurnScope
{
    public TurnBlock TurnBlock { get; set; }
    public TurnState TurnState { get; set; }
    public GameModule Player { get; set; }
    
}