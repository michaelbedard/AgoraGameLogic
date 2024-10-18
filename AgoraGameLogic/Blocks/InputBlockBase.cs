using System;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Blocks;

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
    protected InputBlockBase(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }
    
    public abstract TCommand GetCommandOrThrow(IContext context);
    
    public override async Task<Result> ExecuteAsync(Scope scope)
    {
        try
        {
            var command = GetCommandOrThrow(scope.Context);
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