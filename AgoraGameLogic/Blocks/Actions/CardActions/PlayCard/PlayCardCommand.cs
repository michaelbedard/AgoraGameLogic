using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Actions.Card.PlayFromHand;

public class PlayCardCommand : ActionCommandBase<PlayCardCommand, PlayCardBlock>
{
    public GameModule Card { get; set; }
    
    public PlayCardCommand(PlayCardBlock actionBlockStatementBlock, Scope? scope) : base(actionBlockStatementBlock, scope)
    {
    }

    public override bool Equals(PlayCardCommand command)
    {
        return Target == command.Target && Card == command.Card;
    }

    public override void Perform(PlayCardCommand command, IContext context)
    {
        // animation
        // PushAnimation(new AnimationCommand(GetType(), command.Player, null)).ForAll();

        // logic
        var cardsInHand = command.Target.Fields.Get<List<GameModule>>("Hand");
        cardsInHand.Remove(command.Card);
    }
    
    public override void Revert(PlayCardCommand command, IContext context)
    {
        // animation
        // ActionBlock.PushAnimation(new PlayCardAnimationDto(GetType().ToString(), args, new Dictionary<string, object>()
        // {
        //     {"IsRevert", true}
        // })).ForAll();

        // logic
        var cardsInHand = command.Target.Fields.Get<List<GameModule>>("Hand");
        cardsInHand.Add(command.Card);
    }
    
    public override BaseCommandDto GetDto(PlayCardCommand inputCommand)
    {
        throw new NotImplementedException();
    }
}