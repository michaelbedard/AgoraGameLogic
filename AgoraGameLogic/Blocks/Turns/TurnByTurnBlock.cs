using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Blocks.Turns;

public class TurnByTurnBlock : TurnBlockBlockBase
{
    public TurnByTurnBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        StartBranch = BlockFactory.CreateArrayOrThrow<StatementBlockBase>(buildData.Inputs[0].AsValidArray(), gameData);
        UpdateBranch = BlockFactory.CreateArrayOrThrow<StatementBlockBase>(buildData.Inputs[1].AsValidArray(), gameData);
        EndBranch = BlockFactory.CreateArrayOrThrow<StatementBlockBase>(buildData.Inputs[2].AsValidArray(), gameData);
    }

    public override async Task<Result> ExecuteAsync(IContext context, Scope? scope)
    {
        foreach (var player in Players)
        {
            var currentPlayer = player;
            context.AddOrUpdate("player", ref currentPlayer);
            
            // start
            var executionResult = await ExecuteStart(context, player)
                .ThenAsync(async () => await ExecuteUpdate(context, player))
                .ThenAsync(async () => await ExecuteEnd(context, player));

            if (!executionResult.IsSuccess)
            {
                return Result.Failure(executionResult.Error);
            }
        }
        
        return Result.Success();
    }
}