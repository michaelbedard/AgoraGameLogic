using System.Collections.Generic;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks.Actions.PlayCard;

public class PlayCardBlock : ActionBlock<PlayCardCommand, PlayCardBlock, OnPlayCardBlock>
{
    private Value<GameModule> _target;
    private Value<GameModule> _card;
    
    public PlayCardBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        BehaviorValue = Value<ActionBehavior>.ParseOrThrow(buildData.Inputs[0], gameData);
        _target = Value<GameModule>.ParseOrThrow(buildData.Inputs[1], gameData);
        _card = Value<GameModule>.ParseOrThrow(buildData.Inputs[2], gameData);
    }
    
    protected override Result<PlayCardCommand> GetCommand()
    {
        var target = _target.GetValueOrThrow(Context);
        var card = _card.GetValueOrThrow(Context);

        var options = new Dictionary<string, object>();

        return Result<PlayCardCommand>.Success(new PlayCardCommand(this, Scope)
        {
            Target = target,
            Card = card,
            Options = options
        });
    }
}