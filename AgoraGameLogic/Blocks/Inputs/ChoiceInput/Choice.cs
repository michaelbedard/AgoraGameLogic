using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Inputs;

public class Choice : InputCommandBase<Choice, ChoiceBlock>
{
    public GameModule Player;
    public object[] Choices;
    
    public Choice(ChoiceBlock inputBlock, Scope? scope) : base(inputBlock, scope)
    {
    }

    public override async Task PerformAsync(Choice command, IContext context, object? answer)
    {
        context.AddOrUpdate("Answer", ref answer);

        // AddToHandOption
        if (answer != null && InputBlock.HasOption<AddToHandOption>())
        {
            var hand = Player.Fields.Get<List<GameModule>>("Hand");
            hand.Add((GameModule)answer);
        }
    }

    public override async Task TriggerEventsAsync(Choice command, IContext context, object? answer)
    {
        var args = new object[] { command.Player, command.Choices };
        await InputBlock.TriggerEventsAsync<OnChoiceBlock>(context, args);
    }

    public override void Revert(Choice command, IContext context)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(Choice action)
    {
        throw new NotImplementedException();
    }

    public override BaseCommandDto GetDto(Choice input)
    {
        return new ChoiceInputDto()
        {
            TargetId = Player.Id,
            Choices = Choices,
        };
    }
}