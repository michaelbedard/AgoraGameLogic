using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Logic.Blocks.Actions.Deck.Shuffle;

public class OnShuffleDeckBlock : EventBlockBase<ShuffleDeckCommand>
{
    public OnShuffleDeckBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }

    protected override async Task<Result> TriggerAsync(IContext context, ShuffleDeckCommand command, Scope? scope)
    {
        throw new NotImplementedException();
    }
}