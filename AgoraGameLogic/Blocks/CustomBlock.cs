using AgoraGameLogic.Actors;
using AgoraGameLogic.Interfaces.Actors;
using AgoraGameLogic.Interfaces.Blocks;
using AgoraGameLogic.Services;
using AgoraGameLogic.Utility.BuildData;

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
    private List<Value<object>> _customBlockInputs;
    
    public CustomBlock(BlockBuildData buildData, GameData gameData) : base(buildData, gameData)
    {
        // customBlockType
        _customBlockType = buildData.Inputs[0].ToString();
        
        // customBlockInputs
        _customBlockInputs = new List<Value<object>>();
        foreach (var inputJToken in buildData.Inputs[1])
        {
            _customBlockInputs.Add(Value<object>.ParseOrThrow(inputJToken, gameData));
        }
    }

    public async Task<Result> ExecuteAsync(IContext context, TurnScope? scope)
    {
        // get the custom block data from BlockService
        var customBlockDatakResult = BlockService.GetCustomBlock(_customBlockType);
        if (!customBlockDatakResult.IsSuccess)
        {
            return Result.Failure(customBlockDatakResult.Error);
        }

        if (customBlockDatakResult.Value.CustomBlock is IStatementBlock statementBlock)
        {
            // update
            var contextCopy = context.Copy();
            SetUpContext(contextCopy, customBlockDatakResult.Value.Definition);
            
            // execute the block
            return await statementBlock.ExecuteAsync(context, scope);
        }

        // is not of correct type
        return Result.Failure($"Block {_customBlockType} is used as a statement block, but isn't");
    }

    public Result<bool> IsSatisfied(IContext context)
    {
        // get the custom block data from BlockService
        var customBlockDataResult = BlockService.GetCustomBlock(_customBlockType);
        if (!customBlockDataResult.IsSuccess)
        {
            return Result<bool>.Failure(customBlockDataResult.Error);
        }
        
        if (customBlockDataResult.Value.CustomBlock is IConditionBlock conditionBlock)
        {
            // update
            var contextCopy = context.Copy();
            SetUpContext(contextCopy, customBlockDataResult.Value.Definition);
            
            // execute the block
            return conditionBlock.IsSatisfied(contextCopy);
        }

        // wrong type
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

    #region PRIVATE

    private Result SetUpContext(IContext context, List<CustomBlockData.CustomBlockDefinitionSegment> segments)
    {
        try
        {
            var inputSegments = segments.FindAll(c => c.SegmentType != CustomBlockSegmentType.Text);

            // check if right amount of arguments
            if (inputSegments.Count != _customBlockInputs.Count)
            {
                Result.Failure($"Not right amount of arguments for custom block {_customBlockType}");
            }

            for (var i = 0; i < inputSegments.Count; i++)
            {
                var segment = inputSegments[i];
                var inputValue = _customBlockInputs[i];

                // get input
                var input = inputValue.GetValueOrThrow(context);

                if (segment.SegmentType == CustomBlockSegmentType.Boolean)
                {
                    context.AddOrUpdate(segment.SegmentLabel, (bool)input);
                }
                else
                {
                    // this is a value, let as object
                    context.AddOrUpdate(segment.SegmentLabel, input);
                }
            }

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    #endregion
}