using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Extensions;

namespace AgoraGameLogic.Blocks.Actions.PlayCard;

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