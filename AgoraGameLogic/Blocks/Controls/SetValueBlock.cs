using System;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.Blocks.Controls;

public class SetValueBlock : StatementBlock
{
    private Value<string> _key;
    private Value<object> _value;
    
    public SetValueBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _key = Value<string>.ParseOrThrow(buildData.Inputs[0], gameData);
        _value = Value<object>.ParseOrThrow(buildData.Inputs[1], gameData);
    }
    
    public override async Task<Result> ExecuteAsync()
    {
        try
        {
            var key = _key.GetValueOrThrow(TurnScope.Context);
            var value = _value.GetValueOrThrow(TurnScope.Context);

            TurnScope.Context.AddOrUpdate(key, ref value);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}