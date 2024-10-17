using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.DataObject.ActionCommandDtos;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Blocks.Actions.Deck.Draw;

public class DrawCardCommand : ActionCommand<DrawCardCommand, DrawCardBlock, OnDrawCardBlock>
{
    public GameModule Deck;
    public GameModule TopCard;
    
    public DrawCardCommand(DrawCardBlock actionBlock, Scope? scope) : base(actionBlock, scope)
    {
    }

    public override Result Perform(DrawCardCommand command, IContext context)
    {
        try
        {
            // logic
            var cardsInDeck = Deck.Fields.Get<List<GameModule>>("Cards");
            TopCard = cardsInDeck[0];

            cardsInDeck.RemoveAt(0);
            var numberOfCards = cardsInDeck.Count;
            Deck.Fields.AddOrUpdate("NumberOfCards", ref numberOfCards);
            Target.Fields.Get<List<GameModule>>("Hand").Add(TopCard);

            // animation
            // TODO

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    public override Result Revert(DrawCardCommand command, IContext context)
    {
        throw new NotImplementedException();
    }
    
    public override List<GameModule> GetArgs()
    {
        return new List<GameModule>() { Deck, TopCard };
    }

    public override CommandDto InitializeDto()
    {
        return new DrawCardActionDto()
        {
            TargetId = Target.Id,
            DeckId = Deck.Id
        };
    }
}