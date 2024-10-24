using AgoraGameLogic.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.Blocks.Actions.DrawCard;

public class DrawCardBlock : ActionBlock<DrawCardCommand, DrawCardBlock, OnDrawCardBlock>
{
    private Value<GameModule> _playerValue;
    private Value<GameModule> _deckValue;
    
    public DrawCardBlock(BlockBuildData buildData, GameData gameData) : base (buildData, gameData)
    {
        BehaviorValue = Value<ActionBehavior>.ParseOrThrow(buildData.Inputs[0], gameData);
        _playerValue = Value<GameModule>.ParseOrThrow(buildData.Inputs[1], gameData);
        _deckValue = Value<GameModule>.ParseOrThrow(buildData.Inputs[2], gameData);
    }

    protected override Result<DrawCardCommand> GetCommand()
    {
        var target = _playerValue.GetValueOrThrow(Context);
        var deck = _deckValue.GetValueOrThrow(Context);

        var options = new Dictionary<string, object>();

        return Result<DrawCardCommand>.Success(new DrawCardCommand(this, Scope)
        {
            Target = target,
            Deck = deck,
            Options = options
        });
    }
}