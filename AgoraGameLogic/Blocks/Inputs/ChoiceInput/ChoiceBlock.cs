using System.Collections.Generic;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Inputs.ChoiceInput;

public class ChoiceBlock : InputBlock<ChoiceCommand, ChoiceBlock, OnChoiceBlock>
{
    private Value<GameModule> _target;
    private Value<object[]> _choices;
    
    public ChoiceBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _target = Value<GameModule>.ParseOrThrow(buildData.Inputs[0], gameData);
        _choices = Value<object[]>.ParseOrThrow(buildData.Inputs[1], gameData);
    }

    protected override Result<ChoiceCommand> GetCommand()
    {
        try
        {
            var target = _target.GetValueOrThrow(Context);
            var choices = _choices.GetValueOrThrow(Context);

            var options = new Dictionary<string, object>();

            return Result<ChoiceCommand>.Success(new ChoiceCommand(this, Scope)
            {
                Target = target,
                Choices = choices,
                AnswerIndex = -1,
                Options = options,
            });
        }
        catch (Exception e)
        {
            return Result<ChoiceCommand>.Failure(e.Message);
        }
    }
}