using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Operators;

public class EqualsBlock : ConditionBlockBase
{
    private Value<object> _firstValue;
    private Value<object> _secondValue;

    public EqualsBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        _firstValue = Value<object>.Parse(definition.Inputs[0], gameData);
        _secondValue = Value<object>.Parse(definition.Inputs[1], gameData);
    }

    public override bool IsSatisfied(IContext context)
    {
        var firstValue = _firstValue.GetValue(context);
        var secondValue = _secondValue.GetValue(context);
        
        return firstValue.Equals(secondValue);
    }
}