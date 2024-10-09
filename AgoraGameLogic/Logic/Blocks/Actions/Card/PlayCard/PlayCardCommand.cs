using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;

namespace AgoraGameLogic.Logic.Blocks.Actions.Card.PlayFromHand;

public class PlayCardCommand : BaseActionCommand<PlayCardCommand, PlayCardBlock>
{
    public GameModule Player { get; set; }
    public GameModule Card { get; set; }
    
    public PlayCardCommand(PlayCardBlock actionBlock, Scope? scope) : base(actionBlock, scope)
    {
    }

    public override bool Equals(PlayCardCommand command)
    {
        return Player == command.Player && Card == command.Card;
    }

    public override void Perform(PlayCardCommand command, Context context)
    {
        // animation
        // PushAnimation(new AnimationCommand(GetType(), command.Player, null)).ForAll();

        // logic
        var cardsInHand = command.Player.Fields.Get<List<GameModule>>("Hand");
        cardsInHand.Remove(command.Card);
    }
    
    public override void Revert(PlayCardCommand command, Context context)
    {
        // animation
        // ActionBlock.PushAnimation(new PlayCardAnimationDto(GetType().ToString(), args, new Dictionary<string, object>()
        // {
        //     {"IsRevert", true}
        // })).ForAll();

        // logic
        var cardsInHand = command.Player.Fields.Get<List<GameModule>>("Hand");
        cardsInHand.Add(command.Card);
    }
    
    public override BaseCommandDto GetDto(PlayCardCommand inputCommand)
    {
        throw new NotImplementedException();
    }
}