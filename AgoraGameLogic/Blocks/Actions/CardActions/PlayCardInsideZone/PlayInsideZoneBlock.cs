using AgoraGameLogic.Core.Entities.Utility;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Commands.Actions.Card.PlayInsideZone;

public class PlayInsideZoneBlock : ActionBlockBase<PlayInsideZoneCommand, PlayInsideZoneBlock>
{
    private Value<GameModule> _playerValue;
    private Value<GameModule> _cardValue;
    private Value<GameModule> _zoneValue;
    
    public PlayInsideZoneBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        BehaviorValue = Value<ActionBehavior>.Parse(definition.Inputs[0], gameData);
        _playerValue = Value<GameModule>.Parse(definition.Inputs[1], gameData);
        _cardValue = Value<GameModule>.Parse(definition.Inputs[2], gameData);
        _zoneValue = Value<GameModule>.Parse(definition.Inputs[3], gameData);
    }

    public override PlayInsideZoneCommand GetCommand(IContext context)
    {
        var player = _playerValue.GetValue(context);
        var card = _cardValue.GetValue(context);
        var zone = _zoneValue.GetValue(context);

        var options = new Dictionary<string, object>();

        return new PlayInsideZoneCommand(this, Scope)
        {
            Target = player,
            Card = card,
            Zone = zone,
            Options = options
        };
    }
}