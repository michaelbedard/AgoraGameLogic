using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Options.TurnOptions;

public class ExitConditionTurnBlock : OptionBlockBase
{
    private Value<bool> _exitConditionValue;
    
    public ExitConditionTurnBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _exitConditionValue = Value<bool>.ParseOrThrow(buildData.Inputs[0], gameData);
    }

    public Value<bool> GetExitConditionOrThrow(IContext context)
    {
        return _exitConditionValue;
    }
}