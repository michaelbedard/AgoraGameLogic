using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;

namespace AgoraGameLogic.Logic.Blocks.Actions.Deck.Shuffle;

public class ShuffleDeckCommand : BaseActionCommand<ShuffleDeckCommand, ShuffleDeckBlock>
{
    public GameModule Player;
    public GameModule Deck;
    
    public ShuffleDeckCommand(ShuffleDeckBlock actionBlock, Scope? scope) : base(actionBlock, scope)
    {
    }

    public override void Perform(ShuffleDeckCommand command, Context context)
    {
        // logic
        Deck.Fields.Get<List<GameModule>>("Cards").Shuffle();
    }

    public override void Revert(ShuffleDeckCommand command, Context context)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(ShuffleDeckCommand command)
    {
        return Player == command.Player && Deck == command.Deck;
    }

    public override BaseCommandDto GetDto(ShuffleDeckCommand inputCommand)
    {
        throw new NotImplementedException();
    }
}