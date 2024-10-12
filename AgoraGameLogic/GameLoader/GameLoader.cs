using AgoraGameLogic.Control.Services;
using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Extensions;
using AgoraGameLogic.Logic;
using AgoraGameLogic.Logic.Blocks;
using AgoraGameLogic.Logic.Blocks.Values;
using AgoraGameLogic.Logic.Rules;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Control.GameLoader;

public partial class GameLoader
{
    public Dictionary<GameModule, GameModuleDefinition> LoadGameModules(GameModuleDefinition[] gameModuleDefinitions, StructureDefinition[] structureDefinitions, GameData gameData)
    {
        var gameModulesToDefinition = new Dictionary<GameModule, GameModuleDefinition>();
        
        // add to context
        foreach (var gameModuleDefinition in gameModuleDefinitions)
        {
            var result = BuildGameModule(
                gameModulesToDefinition, 
                gameModuleDefinition, 
                structureDefinitions, 
                gameData,
                null);
            
            foreach (var r in result)
            {
                gameModulesToDefinition[r] = gameModuleDefinition;
            }
        }

        return gameModulesToDefinition;
    }
    
    public void LoadGlobalVariables(KeyValuePairDefinition[] globalVariableDefinitions, GameData gameData)
    {
        // add global var
        foreach (var globalVariableDefinition in globalVariableDefinitions)
        {
            AddGlobalVariableToContext(gameData.GlobalContext, globalVariableDefinition);
        }
        
        // add build-it global var
        AddBuiltInGlobalVariableToContext(gameData.GlobalContext, gameData.Players);
    }

    public void LoadGlobalEvents(BlockDefinition[] globalBlockDefinitions, GameData gameData)
    {
        foreach (var globalBlockDefinition in globalBlockDefinitions)
        {
            var block = BlockFactory.Create(globalBlockDefinition, gameData);

            if (block is EventBlockBase eventBlock)
            {
                gameData.EventService.RegisterGlobalEvent(eventBlock);
            }
        }
    }
    
    public void LoadGameModuleEvents(Dictionary<GameModule, GameModuleDefinition> gameModulesToDefinition,  StructureDefinition[] structureDefinitions, GameData gameData)
    {
        foreach (var entry in gameModulesToDefinition)
        {
            var structureHierarchy = GameLoaderUtility.GetStructureHierarchy(structureDefinitions, entry.Value.Structure);
            var structureBlockDefinitions = structureHierarchy.SelectMany(b => b.Blocks);
            var blockDefinitions = entry.Value.Blocks.Concat(structureBlockDefinitions).ToList();
            
            foreach (var blockDefinition in blockDefinitions)
            {
                var block = BlockFactory.Create(blockDefinition, gameData);

                if (block is EventBlockBase eventBlock)
                {
                    gameData.EventService.RegisterModuleEvent(entry.Key, eventBlock);
                }
            }
        }
    }
    
    public void LoadDescriptions(Dictionary<GameModule, GameModuleDefinition> gameModulesToDefinition, StructureDefinition[] structureDefinitions, GameData gameData)
    {
        foreach (var entry in gameModulesToDefinition)
        {
            var description = entry.Value.Description;
            if (!string.IsNullOrEmpty(entry.Value.Structure))
            {
                var structureHierarchy = GameLoaderUtility.GetStructureHierarchy(structureDefinitions, entry.Value.Structure);
                var structureDescriptionDefinitions = structureHierarchy.Select(b => b.Description);

                foreach (var structureDescriptionDefinition in structureDescriptionDefinitions.Reverse())
                {
                    if (structureDescriptionDefinition.Count > 0)
                    {
                        description = structureDescriptionDefinition;
                        break;
                    }
                }
            }

            if (description.Count > 0)
            {
                gameData.DescriptionService.RegisterDescription(entry.Key, description, gameData);
            }
        }
    }

    public void LoadScoringRules(ScoringRuleDefinition[] scoringRuleDefinitions, GameData gameData)
    {
        foreach (var scoringRuleDefinition in scoringRuleDefinitions)
        {
            var scoringRule = ScoringRuleFactory.Create(scoringRuleDefinition, gameData);
            gameData.ScoringService.RegisterRule(scoringRuleDefinition.Tag, scoringRule);
        }
    }
}