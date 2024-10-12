using AgoraGameLogic.Core.Entities.Utility;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks;

public abstract class InputBlockBase : StatementBlockBase
{
    public InputBlockBase(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
    }
}

public abstract class InputBlockBase<TCommand, TBlock> : InputBlockBase
    where TCommand : InputCommandBase<TCommand, TBlock>
    where TBlock : InputBlockBase<TCommand, TBlock>
{
    protected InputBlockBase(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
    }
    
    public abstract TCommand GetCommand(IContext context);
    protected abstract void CompletePendingRequest(PendingRequest<InputCommandBase> pendingRequest, TCommand command);
    
    public override async Task ExecuteAsync(IContext context, Scope scope)
    {
        var command = GetCommand(context);
        var pendingRequest = PushInput(command);
        CompletePendingRequest(pendingRequest, command);
        
        // wait for response
        await CompletionSource.Task;
    }
}