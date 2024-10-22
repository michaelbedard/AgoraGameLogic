using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Extensions;

namespace AgoraGameLogic.Blocks.Actions.PlayCardInsideZone;

public class OnPlayInsideZoneBlock : EventBlock<PlayInsideZoneCommand>
{
    public OnPlayInsideZoneBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        Blocks = BlockFactory.CreateArrayOrThrow<StatementBlock>(buildData.Inputs[0].AsValidArray(), gameData);
    }

    protected override async Task<Result> TriggerAsyncCore(PlayInsideZoneCommand command)
    {
        Context.AddOrUpdate("Player", ref command.Target);
        Context.AddOrUpdate("Card", ref command.Card);
        Context.AddOrUpdate("Zone", ref command.Zone);

        return await ExecuteSequenceAsync(Blocks);
    }
}