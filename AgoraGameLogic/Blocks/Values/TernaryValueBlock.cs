using System;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Values;

public class TernaryValueBlock : ValueBlockBase
{
    private ConditionBlockBase _condition;
    private Value<object> _trueValue;
    private Value<object> _falseValue;
    
    public TernaryValueBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _condition = BlockFactory.CreateOrThrow<ConditionBlockBase>(buildData.Inputs[0], gameData);
        _trueValue = Value<object>.ParseOrThrow(buildData.Inputs[1], gameData);
        _falseValue = Value<object>.ParseOrThrow(buildData.Inputs[2], gameData);
    }
    
    public override Result<T> GetValue<T>(IContext context)
    {
        try
        {
            if (_condition.IsSatisfiedOrThrow(context))
            {
                var value = (T)_trueValue.GetValueOrThrow(context);
                return Result<T>.Success(value);
            }
            else
            {
                var value = (T)_falseValue.GetValueOrThrow(context);
                return Result<T>.Success(value);
            }
        }
        catch (Exception e)
        {
            return Result<T>.Failure(e.Message);
        }
    }
}