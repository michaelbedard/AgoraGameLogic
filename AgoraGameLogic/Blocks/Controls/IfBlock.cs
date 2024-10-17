using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Entities;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Logic.Blocks.Controls;

public class IfBlock : StatementBlockBase
{
    private ConditionBlockBase _condition;
    private StatementBlockBase[] _trueBranch;
    
    public IfBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _condition = BlockFactory.CreateOrThrow<ConditionBlockBase>(buildData.Inputs[0], gameData);
        _trueBranch = BlockFactory.CreateArrayOrThrow<StatementBlockBase>(buildData.Inputs[1].AsValidArray(), gameData);
    }

    public override async Task<Result> ExecuteAsync(IContext context, Scope? scope)
    {
        try
        {
            if (_condition.IsSatisfiedOrThrow(context))
            {
                return await ExecuteSequenceAsync(_trueBranch, context, scope); // this return a result
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}