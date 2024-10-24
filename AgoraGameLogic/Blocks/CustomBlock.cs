using AgoraGameLogic.Actors;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Interfaces.Blocks;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Extensions;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Blocks;

/// <summary>
///
/// Use this block to call a defined custom block.
/// First arg is the block type, second is a JArray of its inputs
/// 
/// </summary>
public class CustomBlock : Block, IStatementBlock, IConditionBlock
{
    private string _customBlockType;
    private JArray _customBlockInputs;
    
    public CustomBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        _customBlockType = buildData.Inputs[0].ToString();
        _customBlockInputs = buildData.Inputs[1].AsValidArray();
    }

    public async Task<Result> ExecuteAsync(IContext context, TurnScope? scope)
    {
        var customBlockResult = BlockService.GetCustomBlock(_customBlockType);
        if (!customBlockResult.IsSuccess)
        {
            return Result.Failure(customBlockResult.Error);
        }

        if (customBlockResult.Value is IStatementBlock statementBlock)
        {
            // update
            
            // execute the block
            return await statementBlock.ExecuteAsync(context, scope);
        }

        return Result.Failure($"Block {_customBlockType} is used as a statement block, but isn't");
    }

    public Result<bool> IsSatisfied(IContext context)
    {
        var customBlockResult = BlockService.GetCustomBlock(_customBlockType);
        if (!customBlockResult.IsSuccess)
        {
            return Result<bool>.Failure(customBlockResult.Error);
        }

        if (customBlockResult.Value is IConditionBlock conditionBlock)
        {
            // update
            
            // execute the block
            return conditionBlock.IsSatisfied(context);
        }

        return Result<bool>.Failure($"Block {_customBlockType} is used as a condition block, but isn't");
    }

    public bool IsSatisfiedOrThrow(IContext context)
    {
        var isSatisfiedResult = IsSatisfied(context);
        if (!isSatisfiedResult.IsSuccess)
        {
            throw new Exception(isSatisfiedResult.Error);
        }

        return isSatisfiedResult.Value;
    }
}