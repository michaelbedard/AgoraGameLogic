using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;

namespace AgoraGameLogic.Logic.Blocks.Turns.Option;

public class NumberOfActionOption : BaseOptionBlock
{
    private Value<int> _numberOfActionValue;
    
    public NumberOfActionOption(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        _numberOfActionValue = Value<int>.Parse(definition.Inputs[0], gameData);
    }

    public int GetNumberOfAction(Context context)
    {
        return _numberOfActionValue.GetValue(context);
    }
}