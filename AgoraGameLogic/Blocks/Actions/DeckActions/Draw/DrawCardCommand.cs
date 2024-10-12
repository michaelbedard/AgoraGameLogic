using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.DataObject.ActionCommandDtos;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Actions.Deck.Draw;

public class DrawCardCommand : ActionCommandBase<DrawCardCommand, DrawCardBlock>
{
    public GameModule Deck;
    
    public DrawCardCommand(DrawCardBlock actionBlock, Scope? scope) : base(actionBlock, scope)
    {
    }

    public override void Perform(DrawCardCommand command, IContext context)
    {
        // logic
        var cardsInDeck = Deck.Fields.Get<List<GameModule>>("Cards");
        var topCard = cardsInDeck[0];
        
        cardsInDeck.RemoveAt(0);
        var numberOfCards = cardsInDeck.Count;
        Deck.Fields.AddOrUpdate("NumberOfCards", ref numberOfCards);
        Target.Fields.Get<List<GameModule>>("Hand").Add(topCard);
    }

    public override void Revert(DrawCardCommand command, IContext context)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(DrawCardCommand command)
    {
        return Target == command.Target && Deck == command.Deck;
    }

    public override BaseCommandDto GetDto(DrawCardCommand inputCommand)
    {
        return new DrawCardActionDto()
        {
            TargetId = Target.Id,
            DeckId = Deck.Id
        };
    }
}