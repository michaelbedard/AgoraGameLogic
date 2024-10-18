using System;
using System.Collections.Generic;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Dtos;

namespace AgoraGameLogic.Utility.Commands;

public abstract class Command : IEquatable<Command>
{
    public int Id;
    public Type Type;
    public GameModule Target;
    public Dictionary<string, object> Options;
    public Scope? Scope;
    
    public abstract bool Equals(Command command);
    public abstract List<GameModule> GetArgs();
    public abstract CommandDto GetDto();
}