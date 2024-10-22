using System;
using System.Collections.Generic;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Dtos;
using AgoraGameLogic.Dtos.ActionCommandDtos;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Blocks.Actions.PlayCard;

public class PlayCardCommand : ActionCommand<PlayCardCommand, PlayCardBlock, OnPlayCardBlock>
{
    public GameModule Card { get; set; }
    
    public PlayCardCommand(PlayCardBlock actionBlockStatementBlock, TurnScope? scope) : base(actionBlockStatementBlock, scope)
    {
    }

    public override Result Perform()
    {
        try
        {
            // logic
            var cardsInHand = Target.Fields.Get<List<GameModule>>("Hand");
            cardsInHand.Remove(Card);

            return Result.Success();

            // animation
            // PushAnimation(new AnimationCommand(GetType(), command.Player, null)).ForAll();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    public override Result Revert()
    {
        try
        {
            // logic
            var cardsInHand = Target.Fields.Get<List<GameModule>>("Hand");
            cardsInHand.Add(Card);
        
            // animation
            // ActionBlock.PushAnimation(new PlayCardAnimationDto(GetType().ToString(), args, new Dictionary<string, object>()
            // {
            //     {"IsRevert", true}
            // })).ForAll();
        
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
    
    // OVERRIDE UTILS
    
    public override List<GameModule> GetArgs()
    {
        return new List<GameModule>() { Target, Card };
    }

    public override CommandDto GetDtoCore()
    {
        return new PlayCardActionDto()
        {
            CardId = Card.Id
        };
    }
}