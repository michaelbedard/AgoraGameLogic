using AgoraGameLogic.Actors;
using AgoraGameLogic.Dtos;
using AgoraGameLogic.Utility.Enums;
namespace AgoraGameLogic.Utility.Commands;

public abstract class Command : IEquatable<Command>
{
    public int Id;
    public Type Type;
    public GameModule Target;
    public Dictionary<string, object> Options;
    public TurnScope? Scope;
    public bool IsPriority;
    public bool IsCancelable;
    
    
    public abstract bool Equals(Command command);
    public abstract List<GameModule> GetArgs();
    public abstract CommandDto GetDto();
}