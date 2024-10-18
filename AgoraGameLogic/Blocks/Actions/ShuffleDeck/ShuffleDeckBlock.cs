using System.Collections.Generic;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks.Actions.ShuffleDeck;

public class ShuffleDeckBlock : ActionBlockBase<ShuffleDeckCommand, ShuffleDeckBlock, OnShuffleDeckBlock>
{
    private Value<GameModule> _playerValue;
    private Value<GameModule> _deckValue;
    
    public ShuffleDeckBlock(BlockBuildData buildData, GameData gameData) : base (buildData, gameData)
    {
        BehaviorValue = Value<ActionBehavior>.ParseOrThrow(buildData.Inputs[0], gameData);
        _playerValue = Value<GameModule>.ParseOrThrow(buildData.Inputs[1], gameData);
        _deckValue = Value<GameModule>.ParseOrThrow(buildData.Inputs[2], gameData);
    }
    
    public override ShuffleDeckCommand GetCommandOrThrow(IContext context)
    {
        var player = _playerValue.GetValueOrThrow(context);
        var deck = _deckValue.GetValueOrThrow(context);

        var options = new Dictionary<string, object>();

        return new ShuffleDeckCommand(this, Scope)
        {
            Target = player,
            Deck = deck,
            Options = options
        };
    }
}