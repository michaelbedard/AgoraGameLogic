using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Extensions;

namespace AgoraGameLogic.Blocks.Actions.DrawCard;

public class OnDrawCardBlock : EventBlock<DrawCardCommand>
{
    public OnDrawCardBlock(BlockBuildData buildData, GameData gameData) : base (buildData, gameData)
    {
        Blocks = BlockFactory.CreateArrayOrThrow<StatementBlock>(buildData.Inputs[0].AsValidArray(), gameData);
    }

    protected override async Task<Result> TriggerAsyncCore(DrawCardCommand command)
    {
        Context.AddOrUpdate("Player", command.Target);
        Context.AddOrUpdate("Deck", command.Deck);
        Context.AddOrUpdate("Card", command.TopCard);

        return await ExecuteSequenceAsync(Blocks);
    }
}