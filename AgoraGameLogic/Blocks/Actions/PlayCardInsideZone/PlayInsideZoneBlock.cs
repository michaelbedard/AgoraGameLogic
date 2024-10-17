using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Commands.Actions.Card.PlayInsideZone;

public class PlayInsideZoneBlock : ActionBlockBase<PlayInsideZoneCommand, PlayInsideZoneBlock, OnPlayInsideZoneBlock>
{
    private Value<GameModule> _playerValue;
    private Value<GameModule> _cardValue;
    private Value<GameModule> _zoneValue;
    
    public PlayInsideZoneBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        BehaviorValue = Value<ActionBehavior>.ParseOrThrow(buildData.Inputs[0], gameData);
        _playerValue = Value<GameModule>.ParseOrThrow(buildData.Inputs[1], gameData);
        _cardValue = Value<GameModule>.ParseOrThrow(buildData.Inputs[2], gameData);
        _zoneValue = Value<GameModule>.ParseOrThrow(buildData.Inputs[3], gameData);
    }

    public override PlayInsideZoneCommand GetCommandOrThrow(IContext context)
    {
        var player = _playerValue.GetValueOrThrow(context);
        var card = _cardValue.GetValueOrThrow(context);
        var zone = _zoneValue.GetValueOrThrow(context);

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