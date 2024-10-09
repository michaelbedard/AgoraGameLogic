using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Logic.Blocks._dev;
using AgoraGameLogic.Logic.Blocks._options;
using AgoraGameLogic.Logic.Blocks.Actions.Card.PlayFromHand;
using AgoraGameLogic.Logic.Blocks.Actions.Deck.Draw;
using AgoraGameLogic.Logic.Blocks.Actions.Deck.Shuffle;
using AgoraGameLogic.Logic.Blocks.Commands.Actions.Card.PlayInsideZone;
using AgoraGameLogic.Logic.Blocks.Controls;
using AgoraGameLogic.Logic.Blocks.Game;
using AgoraGameLogic.Logic.Blocks.Inputs;
using AgoraGameLogic.Logic.Blocks.Operators;
using AgoraGameLogic.Logic.Blocks.Turns;
using AgoraGameLogic.Logic.Blocks.Values;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Logic.Blocks;

public class BlockFactory
{
    public static T[] CreateArray<T>(BlockDefinition[] blockDefinitions, GameData gameData) where T : BaseBlock
    {
        var blocks = new List<T>();
        foreach (var blockDefinition in blockDefinitions)
        {
            var block = Create(blockDefinition, gameData);

            if (block is T validBlock)
            {
                blocks.Add(validBlock);
            }
            else
            {
                throw new InvalidCastException($"Expected a {typeof(T)} but got {block.GetType().Name}");
            }
        }

        return blocks.ToArray();
    }
    
    public static T[] CreateArray<T>(JArray blockArray, GameData gameData) where T : BaseBlock
    {
        var blocks = new List<T>();
        foreach (var blockToken in blockArray)
        {
            var blockDefinition = BlockDefinition.Parse(blockToken);
            var block = Create(blockDefinition, gameData);

            if (block is T validBlock)
            {
                blocks.Add(validBlock);
            }
            else
            {
                throw new InvalidCastException($"Expected a {typeof(T)} but got {block.GetType().Name}");
            }
        }

        return blocks.ToArray();
    }
    
    public static T Create<T>(JToken blockToken, GameData gameData) where T : BaseBlock
    {
        var blockDefinition = BlockDefinition.Parse(blockToken);
        return Create<T>(blockDefinition, gameData);
    }
    
    public static T Create<T>(BlockDefinition blockDefinition, GameData gameData) where T : BaseBlock
    {
        var block = Create(blockDefinition, gameData);

        if (block is T)
        {
            return (T)block;
        }
        else
        {
            throw new Exception($"Expected a block of type {blockDefinition.Type}");
        }
    }

    public static BaseBlock Create(BlockDefinition blockDefinition, GameData gameData)
    {
        BaseBlock block;
        block = blockDefinition.Type switch
        {
            // Dev
            nameof(LogBlock) => new LogBlock(blockDefinition, gameData),
            
            // Options
            nameof(OnlyTriggerIfTargetedBlock) => new OnlyTriggerIfTargetedBlock(blockDefinition, gameData),
            
            // Action : card
            nameof(PlayCardBlock) => new PlayCardBlock(blockDefinition, gameData),
            nameof(OnPlayCardBlock) => new OnPlayCardBlock(blockDefinition, gameData),
            nameof(PlayInsideZoneBlock) => new PlayInsideZoneBlock(blockDefinition, gameData),
            nameof(OnPlayInsideZoneBlock) => new OnPlayInsideZoneBlock(blockDefinition, gameData),
            
            // Action : deck
            nameof(DrawCardBlock) => new DrawCardBlock(blockDefinition, gameData),
            nameof(OnDrawCardBlock) => new OnDrawCardBlock(blockDefinition, gameData),
            nameof(ShuffleDeckBlock) => new ShuffleDeckBlock(blockDefinition, gameData),
            
            // Input
            nameof(ChoiceBlock) => new ChoiceBlock(blockDefinition, gameData),
            
            // utils
            nameof(ForeachBlock) => new ForeachBlock(blockDefinition, gameData),
            //nameof(ForeachPlayerBlock) => new ForeachPlayerBlock(blockDefinition, gameData),
            nameof(IfBlock) => new IfBlock(blockDefinition, gameData),
            //nameof(RepeatBlock) => new RepeatBlock(blockDefinition, gameData),
            nameof(SetValueBlock) => new SetValueBlock(blockDefinition, gameData),
            
            // Game
            nameof(OnStartGameBlock) => new OnStartGameBlock(blockDefinition, gameData),
            nameof(EndGameBlock) => new EndGameBlock(blockDefinition, gameData),
            
            // Operators
            nameof(EqualsBlock) => new EqualsBlock(blockDefinition, gameData),
            
            // Turns
            nameof(TurnByTurnBlock) => new TurnByTurnBlock(blockDefinition, gameData),
            //nameof(AllTogetherBlock) => new AllTogetherBlock(blockDefinition, gameData),
            
            // Values
            nameof(ContextValueBlock) => new ContextValueBlock(blockDefinition, gameData),
            //nameof(CountValueBlock) => new CountValueBlock(blockDefinition, gameData),
            //nameof(ValueFieldBlock) => new ValueFieldBlock(blockDefinition, gameData),
            //nameof(TernaryValueBlock) => new TernaryValueBlock(blockDefinition, gameData),
            
            _ => throw new Exception($"Don't know how to create block of type '{blockDefinition.Type}'")
        };
        
        return block;
    }
}