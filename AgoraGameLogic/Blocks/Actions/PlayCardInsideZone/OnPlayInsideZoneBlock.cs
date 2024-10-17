using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Logic.Blocks.Commands.Actions.Card.PlayInsideZone;

public class OnPlayInsideZoneBlock : EventBlockBase<PlayInsideZoneCommand>
{
    public OnPlayInsideZoneBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        Blocks = BlockFactory.CreateArrayOrThrow<StatementBlockBase>(buildData.Inputs[0].AsValidArray(), gameData);
    }

    protected override async Task<Result> TriggerAsync(IContext context, PlayInsideZoneCommand command, Scope? scope)
    {
        context.AddOrUpdate("Player", ref command.Target);
        context.AddOrUpdate("Card", ref command.Card);
        context.AddOrUpdate("Zone", ref command.Zone);

        return await ExecuteSequenceAsync(Blocks, context, scope);
    }
}