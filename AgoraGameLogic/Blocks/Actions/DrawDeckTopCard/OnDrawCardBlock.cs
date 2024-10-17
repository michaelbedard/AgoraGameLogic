using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Logic.Blocks.Actions.Deck.Draw;

public class OnDrawCardBlock : EventBlockBase<DrawCardCommand>
{
    public OnDrawCardBlock(BlockBuildData buildData, GameData gameData) : base (buildData, gameData)
    {
        Blocks = BlockFactory.CreateArrayOrThrow<StatementBlockBase>(buildData.Inputs[0].AsValidArray(), gameData);
    }

    protected override async Task<Result> TriggerAsync(IContext context, DrawCardCommand command, Scope? scope)
    {
        context.AddOrUpdate("Player", command.Target);
        context.AddOrUpdate("Deck", command.Deck);
        context.AddOrUpdate("Card", command.TopCard);

        return await ExecuteSequenceAsync(Blocks, context, scope);
    }
}