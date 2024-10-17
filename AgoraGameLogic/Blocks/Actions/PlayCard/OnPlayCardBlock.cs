using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Logic.Blocks.Actions.Card.PlayFromHand;

public class OnPlayCardBlock : EventBlockBase<PlayCardCommand>
{
    public OnPlayCardBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        Blocks = BlockFactory.CreateArrayOrThrow<StatementBlockBase>(buildData.Inputs[0].AsValidArray(), gameData);
    }

    protected override async Task<Result> TriggerAsync(IContext context, PlayCardCommand command, Scope? scope)
    {
        context.AddOrUpdate("Player", command.Target);
        context.AddOrUpdate("Card", command.Card);
        
        return await ExecuteSequenceAsync(Blocks, context, scope);
    }
}