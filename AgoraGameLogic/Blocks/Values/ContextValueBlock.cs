using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Blocks.Values;

public class ContextValueBlock : ValueBlockBase
{
    private Value<string> _bindingName;
    private Value<string[]>? _fields;
    
    public ContextValueBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _bindingName = Value<string>.ParseOrThrow(buildData.Inputs[0], gameData);

        // optional fields to dig in
        if (buildData.Inputs.Count > 1)
        {
            _fields = Value<string[]>.ParseOrThrow(buildData.Inputs[1], gameData);
        }
    }
    
    public override Result<T> GetValue<T>(IContext context)
    {
        try
        {
            var key = _bindingName.GetValueOrThrow(context);
            var result = context.Get<object>(key);
            if (_fields != null)
            {
                if (result is GameModule gameModule)
                {
                    foreach (var field in _fields.GetValueOrThrow(context))
                    {
                        result = gameModule.Fields.Get<object>(field);
                    }
                }
                else
                {
                    throw new Exception($"ContextValueBlock called with fields, but isn't a game module. key: {key}, result : {result}");
                }
            }

            return Result<T>.Success((T)result);
        }
        catch (Exception e)
        {
            return Result<T>.Failure(e.Message);
        }
    }
}