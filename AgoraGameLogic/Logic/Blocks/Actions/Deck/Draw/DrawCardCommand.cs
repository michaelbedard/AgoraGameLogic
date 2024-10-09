using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.DataObject.ActionCommandDtos;
using AgoraGameLogic.Domain.Entities.Models;

namespace AgoraGameLogic.Logic.Blocks.Actions.Deck.Draw;

public class DrawCardCommand : BaseActionCommand<DrawCardCommand, DrawCardBlock>
{
    public GameModule Player;
    public GameModule Deck;
    
    public DrawCardCommand(DrawCardBlock actionBlock, Scope? scope) : base(actionBlock, scope)
    {
    }

    public override void Perform(DrawCardCommand command, Context context)
    {
        // logic
        var cardsInDeck = Deck.Fields.Get<List<GameModule>>("Cards");
        var topCard = cardsInDeck[0];
        
        cardsInDeck.RemoveAt(0);
        var numberOfCards = cardsInDeck.Count;
        Deck.Fields.AddOrUpdate("NumberOfCards", ref numberOfCards);
        Player.Fields.Get<List<GameModule>>("Hand").Add(topCard);
    }

    public override void Revert(DrawCardCommand command, Context context)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(DrawCardCommand command)
    {
        return Player == command.Player && Deck == command.Deck;
    }

    public override BaseCommandDto GetDto(DrawCardCommand inputCommand)
    {
        return new DrawCardActionDto()
        {
            TargetId = Target.Id,
            PlayerId = Player.Id,
            DeckId = Deck.Id
        };
    }
}