using System;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Values;

public class TernaryValueBlock : ValueBlock
{
    private ConditionBlock _condition;
    private Value<object> _trueValue;
    private Value<object> _falseValue;
    
    public TernaryValueBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _condition = BlockFactory.CreateOrThrow<ConditionBlock>(buildData.Inputs[0], gameData);
        _trueValue = Value<object>.ParseOrThrow(buildData.Inputs[1], gameData);
        _falseValue = Value<object>.ParseOrThrow(buildData.Inputs[2], gameData);
    }
    
    protected override Result<T> GetValue<T>()
    {
        try
        {
            if (_condition.IsSatisfiedOrThrow(Context))
            {
                var value = (T)_trueValue.GetValueOrThrow(Context);
                return Result<T>.Success(value);
            }
            else
            {
                var value = (T)_falseValue.GetValueOrThrow(Context);
                return Result<T>.Success(value);
            }
        }
        catch (Exception e)
        {
            return Result<T>.Failure(e.Message);
        }
    }
}