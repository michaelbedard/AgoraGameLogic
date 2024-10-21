using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Extensions;

namespace AgoraGameLogic.Blocks.Controls;

public class ForeachBlock : StatementBlock
{
    private Value<string> _key;
    private Value<IEnumerable<object>> _enumerable;
    private StatementBlock[] _loopBranch;
    
    public ForeachBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _key = Value<string>.ParseOrThrow(buildData.Inputs[0], gameData);
        _enumerable = Value<IEnumerable<object>>.ParseOrThrow(buildData.Inputs[1], gameData);
        _loopBranch = BlockFactory.CreateArrayOrThrow<StatementBlock>(buildData.Inputs[2].AsValidArray(), gameData);
    }

    public override async Task<Result> ExecuteAsync()
    {
        try
        {
            var key = _key.GetValueOrThrow(TurnScope.Context);
            var enumerable = _enumerable.GetValueOrThrow(TurnScope.Context).ToList();

            for (var i = 0; i < enumerable.Count(); i++)
            {
                var contextCopy = TurnScope.Context.Copy();
                var item = enumerable[i];

                contextCopy.AddOrUpdate(key, ref item);

                await ExecuteSequenceOrThrowAsync(_loopBranch, TurnScope);
            }
            
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message, new ErrorBuilder()
            {
                Scope = TurnScope
            });
        }
    }
}