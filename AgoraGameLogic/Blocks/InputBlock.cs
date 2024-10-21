using System;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Blocks;

public abstract class InputBlock : StatementBlock
{
    public InputBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }
}

public abstract class InputBlock<TCommand, TBlock, TEvent> : InputBlock
    where TCommand : InputCommand<TCommand, TBlock, TEvent>
    where TBlock : InputBlock<TCommand, TBlock, TEvent>
    where TEvent : EventBlock
{
    protected InputBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }
    
    protected abstract Result<TCommand> GetCommand();
    
    protected override async Task<Result> ExecuteAsyncCore()
    {
        try
        {
            var commandResult = GetCommand();
            if (!commandResult.IsSuccess)
            {
                return Result.Failure(commandResult.Error);
            }

            // push
            var command = commandResult.Value;
            PushInputOrThrow(command).For(command.Target);

            // wait for response
            await GetOrCreateCompletionSource().Task;

            // return
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}