using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Blocks.Controls;

public class SetValueBlock : StatementBlockBase
{
    private Value<string> _key;
    private Value<object> _value;
    
    public SetValueBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _key = Value<string>.ParseOrThrow(buildData.Inputs[0], gameData);
        _value = Value<object>.ParseOrThrow(buildData.Inputs[1], gameData);
    }
    
    public override async Task<Result> ExecuteAsync(IContext context, Scope? scope)
    {
        try
        {
            var key = _key.GetValueOrThrow(context);
            var value = _value.GetValueOrThrow(context);

            context.AddOrUpdate(key, ref value);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}