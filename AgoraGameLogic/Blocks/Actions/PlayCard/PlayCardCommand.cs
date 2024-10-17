using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.DataObject.ActionCommandDtos;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Blocks.Actions.Card.PlayFromHand;

public class PlayCardCommand : ActionCommand<PlayCardCommand, PlayCardBlock, OnPlayCardBlock>
{
    public GameModule Card { get; set; }
    
    public PlayCardCommand(PlayCardBlock actionBlockStatementBlock, Scope? scope) : base(actionBlockStatementBlock, scope)
    {
    }

    public override Result Perform(PlayCardCommand command, IContext context)
    {
        try
        {
            // logic
            var cardsInHand = command.Target.Fields.Get<List<GameModule>>("Hand");
            cardsInHand.Remove(command.Card);

            return Result.Success();

            // animation
            // PushAnimation(new AnimationCommand(GetType(), command.Player, null)).ForAll();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    public override Result Revert(PlayCardCommand command, IContext context)
    {
        try
        {
            // logic
            var cardsInHand = command.Target.Fields.Get<List<GameModule>>("Hand");
            cardsInHand.Add(command.Card);
        
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

    public override CommandDto InitializeDto()
    {
        return new PlayCardActionDto()
        {
            CardId = Card.Id
        };
    }
}