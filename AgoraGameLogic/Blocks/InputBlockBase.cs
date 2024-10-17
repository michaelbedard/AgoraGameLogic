using AgoraGameLogic.Core.Entities.Utility;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Logic.Blocks;

public abstract class InputBlockBase : StatementBlockBase
{
    public InputBlockBase(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }
}

public abstract class InputBlockBase<TCommand, TBlock, TEvent> : InputBlockBase
    where TCommand : InputCommand<TCommand, TBlock, TEvent>
    where TBlock : InputBlockBase<TCommand, TBlock, TEvent>
    where TEvent : EventBlockBase
{
    // TYODO dfine Scope here
    
    protected InputBlockBase(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }
    
    public abstract TCommand GetCommandOrThrow(IContext context);
    
    public override async Task<Result> ExecuteAsync(IContext context, Scope scope)
    {
        try
        {
            Scope = Scope = scope;
            var command = GetCommandOrThrow(context);
            PushInputOrThrow(command).For(command.Target);

            // wait for response
            await CompletionSource.Task;

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}