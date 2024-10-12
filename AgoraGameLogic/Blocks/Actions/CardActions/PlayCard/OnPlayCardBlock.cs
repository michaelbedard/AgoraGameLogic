using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Logic.Blocks.Actions.Card.PlayFromHand;

public class OnPlayCardBlock : EventBlockBase
{
    public OnPlayCardBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        Blocks = BlockFactory.CreateArray<StatementBlockBase>(definition.Inputs[0].AsValidArray(), gameData);
    }

    protected override async Task TriggerAsync(IContext context, object[] args, Scope? scope)
    {
        context.AddOrUpdate("Player", ref args[0]);
        context.AddOrUpdate("Card", ref args[1]);
        
        await ExecuteSequenceAsync(Blocks, context, scope);
    }
}