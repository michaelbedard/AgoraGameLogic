using AgoraGameLogic.Core.Entities.Utility;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Actions.Card.PlayFromHand;

public class PlayCardBlock : ActionBlockBase<PlayCardCommand, PlayCardBlock>
{
    private Value<GameModule> _player;
    private Value<GameModule> _card;
    
    public PlayCardBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        BehaviorValue = Value<ActionBehavior>.Parse(definition.Inputs[0], gameData);
        _player = Value<GameModule>.Parse(definition.Inputs[1], gameData);
        _card = Value<GameModule>.Parse(definition.Inputs[2], gameData);
    }
    
    public override PlayCardCommand GetCommand(IContext context)
    {
        var player = _player.GetValue(context);
        var card = _card.GetValue(context);

        var options = new Dictionary<string, object>();

        return new PlayCardCommand(this, Scope)
        {
            Target = player,
            Card = card,
            Options = options
        };
    }
}