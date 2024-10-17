using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;

namespace AgoraGameLogic.Logic.Blocks.Controls;

public class ForeachBlock : StatementBlockBase
{
    private Value<string> _key;
    private Value<IEnumerable<object>> _enumerable;
    private StatementBlockBase[] _loopBranch;
    
    public ForeachBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _key = Value<string>.ParseOrThrow(buildData.Inputs[0], gameData);
        _enumerable = Value<IEnumerable<object>>.ParseOrThrow(buildData.Inputs[1], gameData);
        _loopBranch = BlockFactory.CreateArrayOrThrow<StatementBlockBase>(buildData.Inputs[2].AsValidArray(), gameData);
    }

    public override async Task<Result> ExecuteAsync(IContext context, Scope? scope)
    {
        try
        {
            var key = _key.GetValueOrThrow(context);
            var enumerable = _enumerable.GetValueOrThrow(context).ToList();

            for (var i = 0; i < enumerable.Count(); i++)
            {
                var contextCopy = context.Copy();
                var item = enumerable[i];

                contextCopy.AddOrUpdate(key, ref item);

                await ExecuteSequenceOrThrowAsync(_loopBranch, context, scope);
            }
            
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message, new ErrorBuilder()
            {
                Scope = Scope
            });
        }
    }
}