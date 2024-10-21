using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks.Options.InputOptions;
using AgoraGameLogic.Dtos;
using AgoraGameLogic.Dtos.InputCommandDtos;
using AgoraGameLogic.Utility.Commands;
using AgoraGameLogic.Utility.Extensions;

namespace AgoraGameLogic.Blocks.Inputs.ChoiceInput;

public class ChoiceCommand : InputCommand<ChoiceCommand, ChoiceBlock, OnChoiceBlock>
{
    public object[] Choices { get; set; }
    public int AnswerIndex { get; set; }
    
    public ChoiceCommand(ChoiceBlock inputBlock, TurnScope scope) : base(inputBlock, scope)
    {
    }

    public override Result Resolve(object? answer)
    {
        try
        {
            InputBlock.Context.AddOrUpdate("Answer", ref answer);
            
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

    public override object GetDefaultAnswer()
    {
        return Choices.ToList().GetRandom();
    }
    
    public override Result Revert()
    {
        throw new NotImplementedException();
    }

    public override CommandDto GetDtoCore()
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