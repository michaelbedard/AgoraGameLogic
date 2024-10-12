using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Actions.Deck.Draw;

public class DrawCardBlock : ActionBlockBase<DrawCardCommand, DrawCardBlock>
{
    private Value<GameModule> _playerValue;
    private Value<GameModule> _deckValue;
    
    public DrawCardBlock(BlockDefinition definition, GameData gameData) : base (definition, gameData)
    {
        BehaviorValue = Value<ActionBehavior>.Parse(definition.Inputs[0], gameData);
        _playerValue = Value<GameModule>.Parse(definition.Inputs[1], gameData);
        _deckValue = Value<GameModule>.Parse(definition.Inputs[2], gameData);
    }

    public override DrawCardCommand GetCommand(IContext context)
    {
        var player = _playerValue.GetValue(context);
        var deck = _deckValue.GetValue(context);

        var options = new Dictionary<string, object>();

        return new DrawCardCommand(this, Scope)
        {
            Target = player,
            Deck = deck,
            Options = options
        };
    }
}