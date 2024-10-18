using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Options.TurnOptions;

public class NumberOfRoundsTurnOption : OptionBlockBase
{
    private Value<int> _numberOfRoundsValue;
    
    public NumberOfRoundsTurnOption(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _numberOfRoundsValue = Value<int>.ParseOrThrow(buildData.Inputs[0], gameData);
    }
    
    public int GetNumberOfRoundsOrThrow(IContext context)
    {
        var numberOfActionResult = _numberOfRoundsValue.GetValue(context);
        if (!numberOfActionResult.IsSuccess)
        {
            throw new Exception(numberOfActionResult.Error);
        }

        return numberOfActionResult.Value;
    }
}