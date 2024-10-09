using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Entities.Utility;
using AgoraGameLogic.Domain.Extensions;

namespace AgoraGameLogic.Logic.Blocks.Controls;

public class ForeachBlock : BaseStatementBlock
{
    private Value<string> _key;
    private Value<IEnumerable<object>> _enumerable;
    private BaseStatementBlock[] _loopBranch;
    
    public ForeachBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        _key = Value<string>.Parse(definition.Inputs[0], gameData);
        _enumerable = Value<IEnumerable<object>>.Parse(definition.Inputs[1], gameData);
        _loopBranch = BlockFactory.CreateArray<BaseStatementBlock>(definition.Inputs[2].AsValidArray(), gameData);
    }

    public override async Task ExecuteAsync(Context context, Scope? scope)
    {
        var key = _key.GetValue(context);
        var enumerable = _enumerable.GetValue(context).ToList();
        
        for (var i = 0; i < enumerable.Count(); i++)
        {
            var contextCopy = context.Copy();
            var item = enumerable[i];
            
            contextCopy.AddOrUpdate(key, ref item);
            
            await ExecuteSequenceAsync(_loopBranch, context, scope);
        }
    }
}