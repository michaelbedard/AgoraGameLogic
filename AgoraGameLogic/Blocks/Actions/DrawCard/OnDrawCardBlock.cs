using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Extensions;

namespace AgoraGameLogic.Blocks.Actions.DrawCard;

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