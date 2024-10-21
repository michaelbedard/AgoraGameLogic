using System;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Options.TurnOptions;

public class NumberOfActionTurnOption : OptionBlock
{
    private Value<int> _numberOfActionValue;
    
    public NumberOfActionTurnOption(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
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