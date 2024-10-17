using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Blocks.Operators;

public class EqualsBlock : ConditionBlockBase
{
    private Value<object> _firstValue;
    private Value<object> _secondValue;

    public EqualsBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _firstValue = Value<object>.ParseOrThrow(buildData.Inputs[0], gameData);
        _secondValue = Value<object>.ParseOrThrow(buildData.Inputs[1], gameData);
    }

    public override Result<bool> IsSatisfied(IContext context)
    {
        try
        {
            var firstValue = _firstValue.GetValue(context);
            var secondValue = _secondValue.GetValue(context);

            return Result<bool>.Success(firstValue.Equals(secondValue));
        }
        catch (Exception e)
        {
            return Result<bool>.Failure(e.Message);
        }
    }
}