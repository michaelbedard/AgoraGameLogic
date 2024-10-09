using AgoraGameLogic.Core.Entities.Utility;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Enums;

namespace AgoraGameLogic.Logic.Blocks.Actions.Deck.Shuffle;

public class ShuffleDeckBlock : BaseActionBlock<ShuffleDeckCommand, ShuffleDeckBlock>
{
    private Value<GameModule> _playerValue;
    private Value<GameModule> _deckValue;
    
    public ShuffleDeckBlock(BlockDefinition definition, GameData gameData) : base (definition, gameData)
    {
        BehaviorValue = Value<ActionBehavior>.Parse(definition.Inputs[0], gameData);
        _playerValue = Value<GameModule>.Parse(definition.Inputs[1], gameData);
        _deckValue = Value<GameModule>.Parse(definition.Inputs[2], gameData);
    }
    
    public override ShuffleDeckCommand GetCommand(Context context)
    {
        var player = _playerValue.GetValue(context);
        var deck = _deckValue.GetValue(context);

        var options = new Dictionary<string, object>();

        return new ShuffleDeckCommand(this, Scope)
        {
            Target = player,
            Deck = deck,
            Options = options
        };
    }
}