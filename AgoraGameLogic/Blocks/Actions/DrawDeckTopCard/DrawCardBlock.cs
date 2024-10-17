using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Actions.Deck.Draw;

public class DrawCardBlock : ActionBlockBase<DrawCardCommand, DrawCardBlock, OnDrawCardBlock>
{
    private Value<GameModule> _playerValue;
    private Value<GameModule> _deckValue;
    
    public DrawCardBlock(BlockBuildData buildData, GameData gameData) : base (buildData, gameData)
    {
        BehaviorValue = Value<ActionBehavior>.ParseOrThrow(buildData.Inputs[0], gameData);
        _playerValue = Value<GameModule>.ParseOrThrow(buildData.Inputs[1], gameData);
        _deckValue = Value<GameModule>.ParseOrThrow(buildData.Inputs[2], gameData);
    }

    public override DrawCardCommand GetCommandOrThrow(IContext context)
    {
        var target = _playerValue.GetValueOrThrow(context);
        var deck = _deckValue.GetValueOrThrow(context);

        var options = new Dictionary<string, object>();

        return new DrawCardCommand(this, Scope)
        {
            Target = target,
            Deck = deck,
            Options = options
        };
    }
}