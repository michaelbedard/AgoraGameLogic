using AgoraGameLogic.Domain.Entities.DataObject;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Blocks.Inputs;

public class ChoiceCommand : InputCommand<ChoiceCommand, ChoiceBlock, OnChoiceBlock>
{
    public object[] Choices;
    public int AnswerIndex;
    
    public ChoiceCommand(ChoiceBlock inputBlock, Scope? scope) : base(inputBlock, scope)
    {
    }

    public override async Task<Result> PerformAsync(ChoiceCommand command, IContext context, object? answer)
    {
        try
        {
            context.AddOrUpdate("Answer", ref answer);

            // AddToHandOption
            if (answer != null && InputBlock.HasOption<AddToHandOption>())
            {
                var hand = Target.Fields.Get<List<GameModule>>("Hand");
                hand.Add((GameModule)answer);
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    public override Result Revert(ChoiceCommand command, IContext context)
    {
        throw new NotImplementedException();
    }

    public override CommandDto InitializeDto()
    {
        return new ChoiceInputDto()
        {
            TargetId = Target.Id,
            Choices = Choices,
        };
    }

    public override List<GameModule> GetArgs()
    {
        return new List<GameModule>() { Target };
    }
}