using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.DataObject.ActionCommandDtos;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Commands.Actions.Card.PlayInsideZone;

public class PlayInsideZoneCommand : ActionCommandBase<PlayInsideZoneCommand, PlayInsideZoneBlock>
{
    public GameModule Card;
    public GameModule Zone;
    
    public PlayInsideZoneCommand(PlayInsideZoneBlock actionBlockStatementBlock, Scope? scope) : base(actionBlockStatementBlock, scope)
    {
    }

    public override void Perform(PlayInsideZoneCommand command, IContext context)
    {
        // logic
        // ActionBlock.PushAnimation(new PlayCardInsideZoneAnimation(GetType(), args, null)).ForAll();

        // logic
        var cardsInHand = Target.Fields.Get<List<GameModule>>("Hand");
        var cardsInZone = Zone.Fields.Get<List<GameModule>>("Cards");
        
        cardsInHand.Remove(Card);
        cardsInZone.Add(Card);

        var numberOfCardInHand = cardsInHand.Count;
        Zone.Fields.AddOrUpdate("NumberOfCards", ref numberOfCardInHand);
    }

    public override void Revert(PlayInsideZoneCommand command, IContext context)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(PlayInsideZoneCommand actionCommand)
    {
        return Card == actionCommand.Card && Zone == actionCommand.Zone;
    }

    public override BaseCommandDto GetDto(PlayInsideZoneCommand inputCommand)
    {
        return new PlayCardInsideZoneActionDto()
        {
            TargetId = Target.Id,
            CardId = Card.Id,
            ZoneId = Zone.Id,
        };
    }
}