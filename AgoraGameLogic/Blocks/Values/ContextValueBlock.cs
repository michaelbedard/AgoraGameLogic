using System;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Values;

public class ContextValueBlock : ValueBlock
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
    
    protected override Result<T> GetValue<T>()
    {
        try
        {
            var key = _bindingName.GetValueOrThrow(Context);
            var result = Context.Get<object>(key);
            if (_fields != null)
            {
                if (result is GameModule gameModule)
                {
                    foreach (var field in _fields.GetValueOrThrow(Context))
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