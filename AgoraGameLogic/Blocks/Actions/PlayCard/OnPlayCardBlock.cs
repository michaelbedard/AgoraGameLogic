using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Extensions;

namespace AgoraGameLogic.Blocks.Actions.PlayCard;

public class OnPlayCardBlock : EventBlock<PlayCardCommand>
{
    public OnPlayCardBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        Blocks = BlockFactory.CreateArrayOrThrow<StatementBlock>(buildData.Inputs[0].AsValidArray(), gameData);
    }

    protected override async Task<Result> TriggerAsync(TurnScope turnScope, PlayCardCommand command)
    {
        turnScope.Context.AddOrUpdate("Player", command.Target);
        turnScope.Context.AddOrUpdate("Card", command.Card);
        
        return await ExecuteSequenceAsync(Blocks, turnScope);
    }
}