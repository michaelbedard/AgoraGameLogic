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

    protected override async Task<Result> TriggerAsync(Scope scope, DrawCardCommand command)
    {
        scope.Context.AddOrUpdate("Player", command.Target);
        scope.Context.AddOrUpdate("Deck", command.Deck);
        scope.Context.AddOrUpdate("Card", command.TopCard);

        return await ExecuteSequenceAsync(Blocks, scope);
    }
}