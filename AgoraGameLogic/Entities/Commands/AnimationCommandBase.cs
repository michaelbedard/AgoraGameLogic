using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Logic.Blocks;

namespace AgoraGameLogic.Domain.Entities.Models;

public abstract class AnimationCommandBase : CommandBase
{
    
}

public abstract class AnimationCommandBase<TCommand> : AnimationCommandBase 
    where TCommand : AnimationCommandBase<TCommand>
{
    public AnimationCommandBase() 
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