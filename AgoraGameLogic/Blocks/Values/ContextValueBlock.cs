using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Logic.Blocks.Values;

public class ContextValueBlock : ValueBlockBase
{
    private Value<string> _bindingName;
    private Value<string[]>? _fields;
    
    public ContextValueBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        _bindingName = Value<string>.Parse(definition.Inputs[0], gameData);

        if (definition.Inputs.Count > 1)
        {
            _fields = Value<string[]>.Parse(definition.Inputs[1], gameData);
        }
    }
    
    public override T GetValue<T>(IContext context)
    {
        var key = _bindingName.GetValue(context);
        var result = context.Get<object>(key);
        if (_fields != null)
        {
            if (result is GameModule gameModule)
            {
                foreach (var field in _fields.GetValue(context))
                {
                    result = gameModule.Fields.Get<object>(field);
                }
            }
            else
            {
                throw new Exception($"ContextValueBlock called with fields, but isn't a game module. key: {key}, result : {result}");
            }
        }

        return (T)result;
    }
}