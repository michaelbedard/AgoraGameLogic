using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Inputs;

public class ChoiceBlock : InputBlockBase<ChoiceCommand, ChoiceBlock, OnChoiceBlock>
{
    private Value<GameModule> _target;
    private Value<object[]> _choices;
    
    public ChoiceBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _target = Value<GameModule>.ParseOrThrow(buildData.Inputs[0], gameData);
        _choices = Value<object[]>.ParseOrThrow(buildData.Inputs[1], gameData);
    }

    public override ChoiceCommand GetCommandOrThrow(IContext context)
    {
        var target = _target.GetValueOrThrow(context);
        var choices = _choices.GetValueOrThrow(context);
        
        var options = new Dictionary<string, object>();

        return new ChoiceCommand(this, Scope)
        {
            Target = target,
            Choices = choices,
            AnswerIndex = -1,
            Options = options,
        };
    }
}