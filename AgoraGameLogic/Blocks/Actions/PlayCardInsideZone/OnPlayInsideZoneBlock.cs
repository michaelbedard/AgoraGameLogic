using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Extensions;

namespace AgoraGameLogic.Blocks.Actions.PlayCardInsideZone;

public class OnPlayInsideZoneBlock : EventBlockBase<PlayInsideZoneCommand>
{
    public OnPlayInsideZoneBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        Blocks = BlockFactory.CreateArrayOrThrow<StatementBlockBase>(buildData.Inputs[0].AsValidArray(), gameData);
    }

    protected override async Task<Result> TriggerAsync(Scope scope, PlayInsideZoneCommand command)
    {
        scope.Context.AddOrUpdate("Player", ref command.Target);
        scope.Context.AddOrUpdate("Card", ref command.Card);
        scope.Context.AddOrUpdate("Zone", ref command.Zone);

        return await ExecuteSequenceAsync(Blocks, scope);
    }
}