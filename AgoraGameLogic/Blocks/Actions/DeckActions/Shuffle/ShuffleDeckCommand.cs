using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Actions.Deck.Shuffle;

public class ShuffleDeckCommand : ActionCommandBase<ShuffleDeckCommand, ShuffleDeckBlock>
{
    public GameModule Deck;
    
    public ShuffleDeckCommand(ShuffleDeckBlock actionBlock, Scope? scope) : base(actionBlock, scope)
    {
    }

    public override void Perform(ShuffleDeckCommand command, IContext context)
    {
        // logic
        Deck.Fields.Get<List<GameModule>>("Cards").Shuffle();
    }

    public override void Revert(ShuffleDeckCommand command, IContext context)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(ShuffleDeckCommand command)
    {
        return Target == command.Target && Deck == command.Deck;
    }

    public override BaseCommandDto GetDto(ShuffleDeckCommand inputCommand)
    {
        throw new NotImplementedException();
    }
}