using System.Collections.Generic;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks.Actions.PlayCardInsideZone;

public class PlayInsideZoneBlock : ActionBlock<PlayInsideZoneCommand, PlayInsideZoneBlock, OnPlayInsideZoneBlock>
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

    public override PlayInsideZoneCommand GetCommandOrThrow()
    {
        var player = _playerValue.GetValueOrThrow(TurnScope.Context);
        var card = _cardValue.GetValueOrThrow(TurnScope.Context);
        var zone = _zoneValue.GetValueOrThrow(TurnScope.Context);

        var options = new Dictionary<string, object>();

        return new PlayInsideZoneCommand(this, TurnScope)
        {
            Target = player,
            Card = card,
            Zone = zone,
            Options = options
        };
    }
}