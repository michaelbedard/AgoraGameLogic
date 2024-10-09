using AgoraGameLogic.Core.Entities.Utility;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;

namespace AgoraGameLogic.Logic.Blocks;

public abstract class BaseInputBlock : BaseStatementBlock
{
    public BaseInputBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
    }
}

public abstract class BaseInputBlock<TCommand, TBlock> : BaseInputBlock
    where TCommand : BaseInputCommand<TCommand, TBlock>
    where TBlock : BaseInputBlock<TCommand, TBlock>
{
    protected BaseInputBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
    }
    
    public abstract TCommand GetCommand(Context context);
    protected abstract void CompletePendingRequest(PendingRequest<BaseInputCommand> pendingRequest, TCommand command);
    
    public override async Task ExecuteAsync(Context context, Scope scope)
    {
        var command = GetCommand(context);
        var pendingRequest = PushInput(command);
        CompletePendingRequest(pendingRequest, command);
        
        // wait for response
        await CompletionSource.Task;
    }
}