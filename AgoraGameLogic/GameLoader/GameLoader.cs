using System.Collections.Generic;
using System.Linq;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Factories;
using AgoraGameLogic.Utility.BuildData;

namespace AgoraGameLogic.GameLoader;

public partial class GameLoader
{
    public Result<Dictionary<GameModule, GameModuleBuildData>> LoadGameModules(GameModuleBuildData[] gameModuleDefinitions, StructureBuildData[] structureDefinitions, GameData gameData)
    {
        var gameModulesToDefinition = new Dictionary<GameModule, GameModuleBuildData>();
        
        // add to context
        foreach (var gameModuleDefinition in gameModuleDefinitions)
        {
            var gameModuleListResult = BuildGameModule(
                gameModulesToDefinition, 
                gameModuleDefinition, 
                structureDefinitions, 
                gameData,
                null);

            if (!gameModuleListResult.IsSuccess)
            {
                return Result<Dictionary<GameModule, GameModuleBuildData>>.Failure(gameModuleListResult.Error);
            }
            
            foreach (var r in gameModuleListResult.Value)
            {
                gameModulesToDefinition[r] = gameModuleDefinition;
            }
        }

        return Result<Dictionary<GameModule, GameModuleBuildData>>.Success(gameModulesToDefinition);
    }
    
    public Result LoadGlobalVariables(KeyValuePairBuildData[] globalVariableDefinitions, GameData gameData)
    {
        Result addResult;
        
        // add global var
        foreach (var globalVariableDefinition in globalVariableDefinitions)
        {
            addResult = AddGlobalVariableToContext(gameData.GlobalContext, globalVariableDefinition);
            if (!addResult.IsSuccess)
            {
                return Result.Failure(addResult.Error);
            }
        }
        
        // add build-it global var
        addResult = AddBuiltInGlobalVariableToContext(gameData.GlobalContext, gameData.Players);
        if (!addResult.IsSuccess)
        {
            return Result.Failure(addResult.Error);
        }
        
        return Result.Success();
    }

    public Result LoadGlobalEvents(BlockBuildData[] globalBlockDefinitions, GameData gameData)
    {
        foreach (var globalBlockDefinition in globalBlockDefinitions)
        {
            var blockResult = BlockFactory.Create(globalBlockDefinition, gameData);
            if (!blockResult.IsSuccess)
            {
                return Result.Failure(blockResult.Error);
            }

            if (blockResult.Value is EventBlockBase eventBlock)
            {
                gameData.EventService.RegisterGlobalEvent(eventBlock);
            }
        }
        
        return Result.Success();
    }
    
    public Result LoadGameModuleEvents(Dictionary<GameModule, GameModuleBuildData> gameModulesToDefinition,  StructureBuildData[] structureDefinitions, GameData gameData)
    {
        foreach (var entry in gameModulesToDefinition)
        {
            var structureHierarchy = GameLoaderUtility.GetStructureHierarchyOrThrow(structureDefinitions, entry.Value.Structure);
            var structureBlockDefinitions = structureHierarchy.SelectMany(b => b.Blocks);
            var blockDefinitions = entry.Value.Blocks.Concat(structureBlockDefinitions).ToList();
            
            foreach (var blockDefinition in blockDefinitions)
            {
                var blockResult = BlockFactory.Create(blockDefinition, gameData);
                if (!blockResult.IsSuccess)
                {
                    return Result.Failure(blockResult.Error);
                }


                if (blockResult.Value is EventBlockBase eventBlock)
                {
                    gameData.EventService.RegisterModuleEvent(entry.Key, eventBlock);
                }
            }
        }

        return Result.Success();
    }
    
    public Result LoadDescriptions(Dictionary<GameModule, GameModuleBuildData> gameModulesToDefinition, StructureBuildData[] structureDefinitions, GameData gameData)
    {
        foreach (var entry in gameModulesToDefinition)
        {
            var descriptionJArray = entry.Value.Description;
            if (!string.IsNullOrEmpty(entry.Value.Structure))
            {
                var structureHierarchy = GameLoaderUtility.GetStructureHierarchyOrThrow(structureDefinitions, entry.Value.Structure);
                var structureDescriptionDefinitions = structureHierarchy.Select(b => b.Description);

                foreach (var structureDescriptionDefinition in structureDescriptionDefinitions.Reverse())
                {
                    if (structureDescriptionDefinition.Count > 0)
                    {
                        descriptionJArray = structureDescriptionDefinition;
                        break;
                    }
                }
            }

            if (descriptionJArray.Count > 0)
            {
                gameData.DescriptionService.RegisterDescriptionJArray(entry.Key, descriptionJArray, gameData);
            }
        }
        
        return Result.Success();
    }

    public Result LoadScoringRules(ScoringRuleBuildData[] scoringRuleDefinitions, GameData gameData)
    {
        foreach (var scoringRuleDefinition in scoringRuleDefinitions)
        {
            var scoringRuleResult = ScoringRuleFactory.Create(scoringRuleDefinition, gameData);
            if (!scoringRuleResult.IsSuccess)
            {
                return Result.Failure(scoringRuleResult.Error);
            }
            
            gameData.ScoringService.RegisterRule(scoringRuleDefinition.Tag, scoringRuleResult.Value);
        }

        return Result.Success();
    }
}