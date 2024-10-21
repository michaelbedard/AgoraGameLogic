using System;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Actions.ShuffleDeck;

public class OnShuffleDeckBlock : EventBlock<ShuffleDeckCommand>
{
    public OnShuffleDeckBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
    }

    protected override async Task<Result> TriggerAsync(TurnScope context, ShuffleDeckCommand command)
    {
        throw new NotImplementedException();
    }
}