using System;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Extensions;

namespace AgoraGameLogic.Blocks.Controls;

public class IfBlock : StatementBlock
{
    private ConditionBlock _condition;
    private StatementBlock[] _trueBranch;
    
    public IfBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _condition = BlockFactory.CreateOrThrow<ConditionBlock>(buildData.Inputs[0], gameData);
        _trueBranch = BlockFactory.CreateArrayOrThrow<StatementBlock>(buildData.Inputs[1].AsValidArray(), gameData);
    }

    protected override async Task<Result> ExecuteAsyncCore()
    {
        try
        {
            if (_condition.IsSatisfiedOrThrow(Context))
            {
                return await ExecuteSequenceAsync(_trueBranch); // this return a result
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}