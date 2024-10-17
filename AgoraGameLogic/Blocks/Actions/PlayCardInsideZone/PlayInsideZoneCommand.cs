using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.DataObject.ActionCommandDtos;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Blocks.Commands.Actions.Card.PlayInsideZone;

public class PlayInsideZoneCommand : ActionCommand<PlayInsideZoneCommand, PlayInsideZoneBlock, OnPlayInsideZoneBlock>
{
    public GameModule Card;
    public GameModule Zone;
    
    public PlayInsideZoneCommand(PlayInsideZoneBlock actionBlockStatementBlock, Scope? scope) : base(actionBlockStatementBlock, scope)
    {
    }

    public override Result Perform(PlayInsideZoneCommand command, IContext context)
    {
        try
        {
            // logic
            var cardsInHand = Target.Fields.Get<List<GameModule>>("Hand");
            var cardsInZone = Zone.Fields.Get<List<GameModule>>("Cards");

            cardsInHand.Remove(Card);
            cardsInZone.Add(Card);

            var numberOfCardInHand = cardsInHand.Count;
            Zone.Fields.AddOrUpdate("NumberOfCards", ref numberOfCardInHand);

            // logic
            // ActionBlock.PushAnimation(new PlayCardInsideZoneAnimation(GetType(), args, null)).ForAll();
            
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    public override Result Revert(PlayInsideZoneCommand command, IContext context)
    {
        throw new NotImplementedException();
    }

    public override CommandDto InitializeDto()
    {
        return new PlayCardInsideZoneActionDto()
        {
            TargetId = Target.Id,
            CardId = Card.Id,
            ZoneId = Zone.Id,
        };
    }

    public override List<GameModule> GetArgs()
    {
        return new List<GameModule>() { Target, Card, Zone };
    }
}