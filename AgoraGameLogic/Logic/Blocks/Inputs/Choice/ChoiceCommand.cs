using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;

namespace AgoraGameLogic.Logic.Blocks.Inputs;

public class ChoiceCommand : BaseInputCommand<ChoiceCommand, ChoiceBlock>
{
    public GameModule Player;
    public object[] Choices;
    
    public ChoiceCommand(ChoiceBlock inputBlock, Scope? scope) : base(inputBlock, scope)
    {
    }

    public override async Task PerformAsync(ChoiceCommand command, Context context, object? answer)
    {
        context.AddOrUpdate("Answer", ref answer);

        // AddToHandOption
        if (answer != null && InputBlock.HasOption<AddToHandOption>())
        {
            var hand = Player.Fields.Get<List<GameModule>>("Hand");
            hand.Add((GameModule)answer);
        }
    }

    public override async Task TriggerEventsAsync(ChoiceCommand command, Context context, object? answer)
    {
        var args = new object[] { command.Player, command.Choices };
        await InputBlock.TriggerEventsAsync<OnChoiceBlock>(context, args);
    }

    public override void Revert(ChoiceCommand command, Context context)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(ChoiceCommand actionCommand)
    {
        throw new NotImplementedException();
    }

    public override BaseCommandDto GetDto(ChoiceCommand inputCommand)
    {
        return new ChoiceInputDto()
        {
            PlayerId = Player.Id,
            Choices = Choices,
        };
    }
}