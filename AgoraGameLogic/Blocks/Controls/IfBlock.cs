using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Domain.Interfaces;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Logic.Blocks.Controls;

public class IfBlock : StatementBlockBase
{
    private ConditionBlockBase _condition;
    private StatementBlockBase[] _trueBranch;
    
    public IfBlock(BlockDefinition definition, GameData gameData) : base(definition, gameData)
    {
        _condition = BlockFactory.Create<ConditionBlockBase>(definition.Inputs[0], gameData);
        _trueBranch = BlockFactory.CreateArray<StatementBlockBase>(definition.Inputs[1].AsValidArray(), gameData);
    }

    public override async Task ExecuteAsync(IContext context, Scope? scope)
    {
        if (_condition.IsSatisfied(context))
        {
            await ExecuteSequenceAsync(_trueBranch, context, scope);
        }
    }
}