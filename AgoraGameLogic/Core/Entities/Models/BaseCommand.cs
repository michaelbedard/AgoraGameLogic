using AgoraGameLogic.Domain.Entities.DataObject;

namespace AgoraGameLogic.Domain.Entities.Models;

public abstract class BaseCommand
{
    public int Id;
    public Type Type;
    public GameModule Target;
    public Dictionary<string, object> Options;
    public Scope? Scope;
    
    public abstract BaseCommandDto GetDto();
}