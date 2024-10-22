using System.Collections.Generic;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks.Actions.ShuffleDeck;

public class ShuffleDeckBlock : ActionBlock<ShuffleDeckCommand, ShuffleDeckBlock, OnShuffleDeckBlock>
{
    private Value<GameModule> _playerValue;
    private Value<GameModule> _deckValue;
    
    public ShuffleDeckBlock(BlockBuildData buildData, GameData gameData) : base (buildData, gameData)
    {
        BehaviorValue = Value<ActionBehavior>.ParseOrThrow(buildData.Inputs[0], gameData);
        _playerValue = Value<GameModule>.ParseOrThrow(buildData.Inputs[1], gameData);
        _deckValue = Value<GameModule>.ParseOrThrow(buildData.Inputs[2], gameData);
    }
    
    protected override Result<ShuffleDeckCommand> GetCommand()
    {
        var player = _playerValue.GetValueOrThrow(Context);
        var deck = _deckValue.GetValueOrThrow(Context);

        var options = new Dictionary<string, object>();

        return Result<ShuffleDeckCommand>.Success(new ShuffleDeckCommand(this, Scope)
        {
            Target = player,
            Deck = deck,
            Options = options
        });
    }
}