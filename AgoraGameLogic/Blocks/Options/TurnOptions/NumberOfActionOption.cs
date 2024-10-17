using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Blocks.Turns.Option;

public class NumberOfActionOption : OptionBlockBase
{
    private Value<int> _numberOfActionValue;
    
    public NumberOfActionOption(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _numberOfActionValue = Value<int>.ParseOrThrow(buildData.Inputs[0], gameData);
    }

    public int GetNumberOfActionOrThrow(IContext context)
    {
        var numberOfActionResult = _numberOfActionValue.GetValue(context);
        if (!numberOfActionResult.IsSuccess)
        {
            throw new Exception(numberOfActionResult.Error);
        }

        return numberOfActionResult.Value;
    }
}