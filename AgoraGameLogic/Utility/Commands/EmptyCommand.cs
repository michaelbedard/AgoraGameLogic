using System;
using System.Collections.Generic;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Dtos;

namespace AgoraGameLogic.Utility.Commands;

public class EmptyCommand : Command
{
    public override bool Equals(Command command)
    {
        throw new NotImplementedException();
    }

    public override List<GameModule> GetArgs()
    {
        throw new NotImplementedException();
    }

    public override CommandDto GetDto()
    {
        throw new NotImplementedException();
    }
}