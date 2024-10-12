using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Turns;

public class TurnByTurnBlock : TurnBlockBlockBase
{
    public TurnByTurnBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        StartBranch = BlockFactory.CreateArray<StatementBlockBase>(definition.Inputs[0].AsValidArray(), gameData);
        UpdateBranch = BlockFactory.CreateArray<StatementBlockBase>(definition.Inputs[1].AsValidArray(), gameData);
        EndBranch = BlockFactory.CreateArray<StatementBlockBase>(definition.Inputs[2].AsValidArray(), gameData);
    }

    public override async Task ExecuteAsync(IContext context, Scope? scope)
    {
        foreach (var player in Players)
        {
            var currentPlayer = player;
            context.AddOrUpdate("player", ref currentPlayer);
            
            // start
            await ExecuteStart(context, player);
            
            // update
            await ExecuteUpdate(context, player);
            
            // end
            await ExecuteEnd(context, player);
        }
    }
}