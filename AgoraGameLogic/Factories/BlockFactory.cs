using System;
using System.Collections.Generic;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Blocks.Actions.DrawCard;
using AgoraGameLogic.Blocks.Actions.PlayCard;
using AgoraGameLogic.Blocks.Actions.PlayCardInsideZone;
using AgoraGameLogic.Blocks.Actions.ShuffleDeck;
using AgoraGameLogic.Blocks.Controls;
using AgoraGameLogic.Blocks.Dev;
using AgoraGameLogic.Blocks.Game;
using AgoraGameLogic.Blocks.Game.StartGame;
using AgoraGameLogic.Blocks.Inputs.ChoiceInput;
using AgoraGameLogic.Blocks.Operators;
using AgoraGameLogic.Blocks.Options;
using AgoraGameLogic.Blocks.Options.TurnOptions;
using AgoraGameLogic.Blocks.Turns;
using AgoraGameLogic.Blocks.Values;
using AgoraGameLogic.Utility.BuildData;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Factories;

public class BlockFactory
{
    /// <summary>
    /// Attempts to create an array of blocks from BlockBuildData and returns a Result containing the array or an error message.
    /// </summary>
    public static Result<T[]> CreateArray<T>(BlockBuildData[] blockBuildDataArray, GameData gameData) where T : BlockBase
    {
        try
        {
            var blocks = new List<T>();

            foreach (var blockDefinition in blockBuildDataArray)
            {
                var blockResult = Create(blockDefinition, gameData);
                if (!blockResult.IsSuccess)
                {
                    return Result<T[]>.Failure(blockResult.Error);
                }
                

                if (blockResult.Value is T validBlock)
                {
                    blocks.Add(validBlock);
                }
                else
                {
                    return Result<T[]>.Failure(
                        $"Invalid cast: Expected a {typeof(T).Name} but got {blockResult.Value.GetType().Name}"
                    );
                }
            }

            return Result<T[]>.Success(blocks.ToArray());
        }
        catch (Exception ex)
        {
            return Result<T[]>.Failure($"Exception occurred: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Attempts to create an array of blocks from a JArray and returns a Result containing the array or an error message.
    /// </summary>
    public static Result<T[]> CreateArray<T>(JArray blockArray, GameData gameData) where T : BlockBase
    {
        try
        {
            var blocks = new List<T>();

            foreach (var blockToken in blockArray)
            {
                // get blockBuildData
                var blockBuildDataResult = BlockBuildData.Parse(blockToken);
                if (!blockBuildDataResult.IsSuccess)
                {
                    return Result<T[]>.Failure(blockBuildDataResult.Error);
                }
                
                // create block
                var blockResult = Create(blockBuildDataResult.Value, gameData);
                if (!blockResult.IsSuccess)
                {
                    return Result<T[]>.Failure(blockResult.Error);
                }

                // add block to the list
                if (blockResult.Value is T validBlock)
                {
                    blocks.Add(validBlock);
                }
                else
                {
                    return Result<T[]>.Failure($"Invalid cast: Expected a {typeof(T).Name} but got {blockResult.Value.GetType().Name}");
                }
            }

            return Result<T[]>.Success(blocks.ToArray());
        }
        catch (Exception ex)
        {
            return Result<T[]>.Failure($"Exception occurred: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Creates an array of blocks from a BlockBuildData[] and throws an exception if the operation fails.
    /// </summary>
    public static T[] CreateArrayOrThrow<T>(BlockBuildData[] blockBuildDataArray, GameData gameData) where T : BlockBase
    {
        var createArrayResult = CreateArray<T>(blockBuildDataArray, gameData);
        if (!createArrayResult.IsSuccess)
        {
            throw new Exception(createArrayResult.Error);
        }

        return createArrayResult.Value;
    }
    
    /// <summary>
    /// Creates an array of blocks from a JArray and throws an exception if the operation fails.
    /// </summary>
    public static T[] CreateArrayOrThrow<T>(JArray blockArray, GameData gameData) where T : BlockBase
    {
        var createArrayResult = CreateArray<T>(blockArray, gameData);
        if (!createArrayResult.IsSuccess)
        {
            throw new Exception(createArrayResult.Error);
        }

        return createArrayResult.Value;
    }

    /// <summary>
    /// Attempts to create a block of the specified type from a JToken.
    /// </summary>
    public static Result<T> Create<T>(JToken blockToken, GameData gameData) where T : BlockBase
    {
        var blockDefinitionResult = BlockBuildData.Parse(blockToken);
        if (!blockDefinitionResult.IsSuccess)
        {
            return Result<T>.Failure(blockDefinitionResult.Error);
        }

        return Create<T>(blockDefinitionResult.Value, gameData);
    }
    
    // CAN WE MERGE BOTH???
    
    /// <summary>
    /// Attempts to create a block of the specified type from BlockBuildData.
    /// </summary>
    public static Result<T> Create<T>(BlockBuildData blockBuildData, GameData gameData) where T : BlockBase
    {
        try
        {
            var blockResult = Create(blockBuildData, gameData);
            if (!blockResult.IsSuccess)
            {
                return Result<T>.Failure(blockResult.Error);
            }

            if (blockResult.Value is T validBlock)
            {
                return Result<T>.Success(validBlock);
            }
            else
            {
                return Result<T>.Failure(
                    $"Invalid cast: Expected a block of type {typeof(T).Name} but got {blockResult.GetType().Name}."
                );
            }
        }
        catch (Exception ex)
        {
            return Result<T>.Failure($"Exception occurred while creating the block: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Attempts to create a block of the specified type from a JToken.
    /// </summary>
    public static T CreateOrThrow<T>(JToken blockToken, GameData gameData) where T : BlockBase
    {
        var blockResult = Create<T>(blockToken, gameData);
        if (!blockResult.IsSuccess)
        {
            throw new Exception(blockResult.Error);
        }

        return blockResult.Value;
    }

    /// <summary>
    /// Attempts to create a block from the provided BlockBuildData based on its type.
    /// </summary>
    public static Result<BlockBase> Create(BlockBuildData blockBuildData, GameData gameData)
    {
        try
        {
            BlockBase? block;
            block = blockBuildData.Type switch
            {
                // Dev
                nameof(LogBlock) => new LogBlock(blockBuildData, gameData),

                // Options
                nameof(OnlyTriggerIfTargetedBlock) => new OnlyTriggerIfTargetedBlock(blockBuildData, gameData),
                nameof(NumberOfActionTurnOption) => new NumberOfActionTurnOption(blockBuildData, gameData),

                // Action : card
                nameof(PlayCardBlock) => new PlayCardBlock(blockBuildData, gameData),
                nameof(OnPlayCardBlock) => new OnPlayCardBlock(blockBuildData, gameData),
                nameof(PlayInsideZoneBlock) => new PlayInsideZoneBlock(blockBuildData, gameData),
                nameof(OnPlayInsideZoneBlock) => new OnPlayInsideZoneBlock(blockBuildData, gameData),

                // Action : deck
                nameof(DrawCardBlock) => new DrawCardBlock(blockBuildData, gameData),
                nameof(OnDrawCardBlock) => new OnDrawCardBlock(blockBuildData, gameData),
                nameof(ShuffleDeckBlock) => new ShuffleDeckBlock(blockBuildData, gameData),

                // Input
                nameof(ChoiceBlock) => new ChoiceBlock(blockBuildData, gameData),

                // utils
                nameof(ForeachBlock) => new ForeachBlock(blockBuildData, gameData),
                //nameof(ForeachPlayerBlock) => new ForeachPlayerBlock(blockDefinition, gameData),
                nameof(IfBlock) => new IfBlock(blockBuildData, gameData),
                //nameof(RepeatBlock) => new RepeatBlock(blockDefinition, gameData),
                nameof(SetValueBlock) => new SetValueBlock(blockBuildData, gameData),

                // Game
                nameof(OnStartGameBlock) => new OnStartGameBlock(blockBuildData, gameData),
                nameof(EndGameBlock) => new EndGameBlock(blockBuildData, gameData),

                // Operators
                nameof(EqualsBlock) => new EqualsBlock(blockBuildData, gameData),

                // Turns
                nameof(TurnByTurnBlock) => new TurnByTurnBlock(blockBuildData, gameData),
                //nameof(AllTogetherBlock) => new AllTogetherBlock(blockDefinition, gameData),

                // Values
                nameof(ContextValueBlock) => new ContextValueBlock(blockBuildData, gameData),
                //nameof(CountValueBlock) => new CountValueBlock(blockDefinition, gameData),
                //nameof(ValueFieldBlock) => new ValueFieldBlock(blockDefinition, gameData),
                nameof(TernaryValueBlock) => new TernaryValueBlock(blockBuildData, gameData),

                // default
                _ => null
            };
            
            if (block == null)
            {
                return Result<BlockBase>.Failure($"Don't know how to create block of type '{blockBuildData.Type}'");
            }

            return Result<BlockBase>.Success(block);
        }
        catch (Exception e)
        {
            return Result<BlockBase>.Failure($"Exception occurred while creating block: {e.Message}");
        }
    }
}