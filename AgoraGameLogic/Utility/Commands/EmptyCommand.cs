using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;

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