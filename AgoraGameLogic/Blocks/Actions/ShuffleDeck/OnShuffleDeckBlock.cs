using System;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Actions.ShuffleDeck;

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