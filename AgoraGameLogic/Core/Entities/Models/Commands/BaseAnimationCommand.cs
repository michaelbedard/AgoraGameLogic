using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Logic.Blocks;

namespace AgoraGameLogic.Domain.Entities.Models;

public abstract class BaseAnimationCommand : BaseCommand
{
    
}

public abstract class BaseAnimationCommand<TCommand> : BaseAnimationCommand 
    where TCommand : BaseAnimationCommand<TCommand>
{
    public BaseAnimationCommand() 
    {
        Type = typeof(TCommand);
    }
    
    public abstract BaseCommandDto GetDto(TCommand animationCommand);
    
    public override BaseCommandDto GetDto()
    {
        var temp = GetDto((TCommand)this);
        temp.Key = nameof(TCommand);
        temp.Options = Options;

        return temp;
    }
}