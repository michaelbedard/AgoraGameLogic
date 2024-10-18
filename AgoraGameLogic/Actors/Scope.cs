using AgoraGameLogic.Blocks;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Actors;

public class Scope
{
    public IContext Context { get; set; }
    public TurnBlockBlockBase TurnBlock { get; set; }
    public TurnState TurnState { get; set; }
    public GameModule Player { get; set; }
}