using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;

namespace AgoraGameLogic.Logic.Blocks.Controls;

public class SetValueBlock : BaseStatementBlock
{
    private Value<string> _key;
    private Value<object> _value;
    
    public SetValueBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        _key = Value<string>.Parse(definition.Inputs[0], gameData);
        _value = Value<object>.Parse(definition.Inputs[1], gameData);
    }
    
    public override async Task ExecuteAsync(Context context, Scope? scope)
    {
        var key = _key.GetValue(context);
        var value = _value.GetValue(context);
        
        context.AddOrUpdate(key, ref value);
    }
}