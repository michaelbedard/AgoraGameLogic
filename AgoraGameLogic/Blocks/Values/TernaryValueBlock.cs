using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Values;

public class TernaryValueBlock : ValueBlockBase
{
    private ConditionBlockBase _condition;
    private Value<object> _trueValue;
    private Value<object> _falseValue;
    
    public TernaryValueBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        _condition = BlockFactory.Create<ConditionBlockBase>(definition.Inputs[0], gameData);
        _trueValue = Value<object>.Parse(definition.Inputs[1], gameData);
        _falseValue = Value<object>.Parse(definition.Inputs[2], gameData);
    }
    
    public override T GetValue<T>(IContext context)
    {
        if (_condition.IsSatisfied(context))
        {
            return (T)_trueValue.GetValue(context);
        }
        else
        {
            return (T)_falseValue.GetValue(context);
        }
    }
}