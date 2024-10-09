using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Logic.Blocks.Actions.Card.PlayFromHand;

public class OnPlayCardBlock : BaseEventBlock
{
    public OnPlayCardBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        Blocks = BlockFactory.CreateArray<BaseStatementBlock>(definition.Inputs[0].AsValidArray(), gameData);
    }

    protected override async Task TriggerAsync(Context context, object[] args, Scope? scope)
    {
        context.AddOrUpdate("Player", ref args[0]);
        context.AddOrUpdate("Card", ref args[1]);
        
        await ExecuteSequenceAsync(Blocks, context, scope);
    }
}