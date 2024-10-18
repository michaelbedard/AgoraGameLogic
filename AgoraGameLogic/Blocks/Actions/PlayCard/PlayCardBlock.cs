using System.Collections.Generic;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks.Actions.PlayCard;

public class PlayCardBlock : ActionBlockBase<PlayCardCommand, PlayCardBlock, OnPlayCardBlock>
{
    private Value<GameModule> _target;
    private Value<GameModule> _card;
    
    public PlayCardBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        BehaviorValue = Value<ActionBehavior>.ParseOrThrow(buildData.Inputs[0], gameData);
        _target = Value<GameModule>.ParseOrThrow(buildData.Inputs[1], gameData);
        _card = Value<GameModule>.ParseOrThrow(buildData.Inputs[2], gameData);
    }
    
    public override PlayCardCommand GetCommandOrThrow(IContext context)
    {
        var target = _target.GetValueOrThrow(context);
        var card = _card.GetValueOrThrow(context);

        var options = new Dictionary<string, object>();

        return new PlayCardCommand(this, Scope)
        {
            Target = target,
            Card = card,
            Options = options
        };
    }
}