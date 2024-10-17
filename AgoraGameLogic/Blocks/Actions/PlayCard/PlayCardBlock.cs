using AgoraGameLogic.Core.Entities.Utility;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Actions.Card.PlayFromHand;

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