using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.DataObject.ActionCommandDtos;
using AgoraGameLogic.Domain.Entities.Models;

namespace AgoraGameLogic.Logic.Blocks.Commands.Actions.Card.PlayInsideZone;

public class PlayInsideZoneCommand : BaseActionCommand<PlayInsideZoneCommand, PlayInsideZoneBlock>
{
    public GameModule Card;
    public GameModule Zone;
    
    public PlayInsideZoneCommand(PlayInsideZoneBlock actionBlock, Scope? scope) : base(actionBlock, scope)
    {
    }

    public override void Perform(PlayInsideZoneCommand command, Context context)
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

    public override void Revert(PlayInsideZoneCommand command, Context context)
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