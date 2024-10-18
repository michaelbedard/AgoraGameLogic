using System;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Extensions;

namespace AgoraGameLogic.Blocks.Controls;

public class IfBlock : StatementBlockBase
{
    private ConditionBlockBase _condition;
    private StatementBlockBase[] _trueBranch;
    
    public IfBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _condition = BlockFactory.CreateOrThrow<ConditionBlockBase>(buildData.Inputs[0], gameData);
        _trueBranch = BlockFactory.CreateArrayOrThrow<StatementBlockBase>(buildData.Inputs[1].AsValidArray(), gameData);
    }

    protected override async Task<Result> ExecuteAsync(IContext context)
    {
        try
        {
            if (_condition.IsSatisfiedOrThrow(context))
            {
                return await ExecuteSequenceAsync(_trueBranch, context); // this return a result
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}