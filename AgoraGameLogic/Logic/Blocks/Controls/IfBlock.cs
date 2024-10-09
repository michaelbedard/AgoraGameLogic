using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Logic.Blocks.Controls;

public class IfBlock : BaseStatementBlock
{
    private BaseConditionBlock _condition;
    private BaseStatementBlock[] _trueBranch;
    
    public IfBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        _condition = BlockFactory.Create<BaseConditionBlock>(definition.Inputs[0], gameData);
        _trueBranch = BlockFactory.CreateArray<BaseStatementBlock>(definition.Inputs[1].AsValidArray(), gameData);
    }

    public override async Task ExecuteAsync(Context context, Scope? scope)
    {
        if (_condition.IsSatisfied(context))
        {
            await ExecuteSequenceAsync(_trueBranch, context, scope);
        }
    }
}