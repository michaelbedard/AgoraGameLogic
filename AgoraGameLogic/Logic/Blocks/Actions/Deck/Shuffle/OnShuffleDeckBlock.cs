using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Logic.Blocks.Values;

namespace AgoraGameLogic.Logic.Blocks.Actions.Deck.Shuffle;

public class OnShuffleDeckBlock : BaseEventBlock
{
    public OnShuffleDeckBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
    }

    protected override async Task TriggerAsync(Context context, object[] args, Scope? scope)
    {
        throw new NotImplementedException();
    }
}