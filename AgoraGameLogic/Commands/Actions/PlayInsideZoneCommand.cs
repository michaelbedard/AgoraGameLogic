using System;
using System.Collections.Generic;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Dtos;
using AgoraGameLogic.Dtos.ActionCommandDtos;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Blocks.Actions.PlayCardInsideZone;

public class PlayInsideZoneCommand : ActionCommand<PlayInsideZoneCommand, PlayInsideZoneBlock, OnPlayInsideZoneBlock>
{
    public GameModule Card;
    public GameModule Zone;
    
    public PlayInsideZoneCommand(PlayInsideZoneBlock actionBlockStatementBlock, TurnScope? scope) : base(actionBlockStatementBlock, scope)
    {
    }

    public override Result Perform()
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

    public override Result Revert()
    {
        throw new NotImplementedException();
    }

    public override CommandDto GetDtoCore()
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