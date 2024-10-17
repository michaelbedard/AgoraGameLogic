using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Logic.Blocks;

namespace AgoraGameLogic.Domain.Entities.Models;

public abstract class AnimationCommand : Command
{
    
}

public abstract class AnimationCommand<TCommand> : AnimationCommand 
    where TCommand : AnimationCommand<TCommand>
{
    public AnimationCommand() 
    {
        Type = typeof(TCommand);
    }
    
    public abstract CommandDto GetDto(TCommand animationCommand);
    
    public override CommandDto GetDto()
    {
        var temp = GetDto((TCommand)this);
        temp.Key = nameof(TCommand);
        temp.Options = Options;

        return temp;
    }
}