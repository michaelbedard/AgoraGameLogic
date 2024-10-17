using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;
using Newtonsoft.Json.Serialization;

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
            var firstResult= _firstValue.GetValue(context);
            if (!firstResult.IsSuccess)
            {
                return Result<bool>.Failure(firstResult.Error);
            }
            
            var secondResult = _secondValue.GetValue(context);
            if (!secondResult.IsSuccess)
            {
                return Result<bool>.Failure(secondResult.Error);
            }

            return Result<bool>.Success(firstResult.Value.Equals(secondResult.Value));
        }
        catch (Exception e)
        {
            return Result<bool>.Failure($"Unexpected Error: {e.Message}", new ErrorBuilder()
            {
                ClassName = nameof(EqualsBlock)
            });
        }
    }
}