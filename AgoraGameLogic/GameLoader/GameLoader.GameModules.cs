using System;
using System.Collections.Generic;
using System.Linq;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.GameLoader;

public partial class GameLoader
{
    public Result<List<GameModule>> BuildGameModule(
        Dictionary<GameModule, GameModuleBuildData> gameModulesToDefinition,
        GameModuleBuildData gameModuleBuildData,
        StructureBuildData[] structureDefinitions,
        GameData gameData, 
        GameModule? parent = null)
    {
        var structureHierarchy = GetStructureHierarchy(structureDefinitions, gameModuleBuildData.Structure);
        var structureFields = GetStructureFields(structureHierarchy);

        var extendedName = ExtendName(gameModuleBuildData.Name, parent);
        var names = GetNamesForModule(extendedName, gameModuleBuildData, gameData.NumberOfPlayers);
        
        var buildGameModules = new List<GameModule>();

        foreach (var name in names)
        {
            var gameModule = CreateGameModule(name, gameModuleBuildData, structureHierarchy);
            buildGameModules.Add(gameModule);

            if (parent == null)
                gameData.GlobalContext.AddOrUpdate(name, ref gameModule);

            HandleModuleType(gameModule, gameModuleBuildData, gameModulesToDefinition, 
                structureDefinitions, gameData, parent);

            AddStructureAndBuildFields(gameModule, structureFields, gameModuleBuildData);
        }

        return Result<List<GameModule>>.Success(buildGameModules);
    }

    private List<StructureBuildData> GetStructureHierarchy(StructureBuildData[] structureDefinitions, string structure)
    {
        return GameLoaderUtility.GetStructureHierarchyOrThrow(structureDefinitions, structure);
    }

    private List<KeyValuePairBuildData> GetStructureFields(List<StructureBuildData> structureHierarchy)
    {
        return structureHierarchy.SelectMany(s => s.Fields).ToList();
    }

    private string ExtendName(string baseName, GameModule? parent)
    {
        return parent == null ? baseName : $"{parent.Name}_{baseName}";
    }

    private string[] GetNamesForModule(string extendedName, GameModuleBuildData gameModuleBuildData, int numberOfPlayers)
    {
        return GameLoaderUtility.GetNamesForGameModuleOrThrow(extendedName, gameModuleBuildData, numberOfPlayers).ToArray();
    }

    private GameModule CreateGameModule(string name, GameModuleBuildData gameModuleBuildData,
        List<StructureBuildData> structureHierarchy)
    {
        return new GameModule(
            name,
            gameModuleBuildData.Name,
            gameModuleBuildData.Type,
            structureHierarchy.Select(s => s.Name).ToArray());
    }

    private void HandleModuleType(
        GameModule gameModule, 
        GameModuleBuildData gameModuleBuildData, 
        Dictionary<GameModule, GameModuleBuildData> gameModulesToDefinition, 
        StructureBuildData[] structureDefinitions, 
        GameData gameData, 
        GameModule? parent)
    {
        switch (gameModule.Type)
        {
            case GameModuleType.Player:
                HandlePlayerModule(gameModule, gameModuleBuildData, gameModulesToDefinition, structureDefinitions, gameData);
                break;
            case GameModuleType.Deck:
                HandleDeckModule(gameModule, gameModuleBuildData, gameModulesToDefinition, structureDefinitions, gameData);
                break;
            case GameModuleType.Zone:
                HandleZoneModule(gameModule);
                break;
        }
    }

    private void HandlePlayerModule(
        GameModule gameModule, 
        GameModuleBuildData gameModuleBuildData, 
        Dictionary<GameModule, GameModuleBuildData> gameModulesToDefinition, 
        StructureBuildData[] structureDefinitions, 
        GameData gameData)
    {
        var modules = BuildChildModules(gameModuleBuildData.Modules, gameModulesToDefinition, structureDefinitions, gameData, gameModule);

        var hand = new List<GameModule>();
        gameModule.Fields.AddOrUpdate("Modules", ref modules);
        gameModule.Fields.AddOrUpdate("Hand", ref hand);
    }

    private void HandleDeckModule(
        GameModule gameModule, 
        GameModuleBuildData gameModuleBuildData, 
        Dictionary<GameModule, GameModuleBuildData> gameModulesToDefinition, 
        StructureBuildData[] structureDefinitions, 
        GameData gameData)
    {
        var cards = BuildChildModules(gameModuleBuildData.Cards, gameModulesToDefinition, structureDefinitions, gameData, gameModule);
        var numberOfCards = cards.Count;
        
        gameModule.Fields.AddOrUpdate("Cards", ref cards);
        gameModule.Fields.AddOrUpdate("NumberOfCards", ref numberOfCards);
    }

    private void HandleZoneModule(GameModule gameModule)
    {
        var cards = new List<GameModule>();
        var numberOfCards = cards.Count;
        gameModule.Fields.AddOrUpdate("Cards", ref cards);
        gameModule.Fields.AddOrUpdate("NumberOfCards", ref numberOfCards);
    }

    private List<GameModule> BuildChildModules(
        GameModuleBuildData[] childModules, 
        Dictionary<GameModule, GameModuleBuildData> gameModulesToDefinition, 
        StructureBuildData[] structureDefinitions, 
        GameData gameData, 
        GameModule parent)
    {
        var modules = new List<GameModule>();

        foreach (var moduleData in childModules)
        {
            var result = BuildGameModule(gameModulesToDefinition, moduleData, structureDefinitions, gameData, parent);

            if (!result.IsSuccess)
                throw new InvalidOperationException(result.Error);

            foreach (var childGameModule in result.Value)
            {
                modules.Add(childGameModule);
                gameModulesToDefinition[childGameModule] = moduleData;
                parent.Fields.AddOrUpdate(moduleData.Name, childGameModule);
            }
        }

        return modules;
    }

    private void AddStructureAndBuildFields(
        GameModule gameModule, 
        List<KeyValuePairBuildData> structureFields, 
        GameModuleBuildData gameModuleBuildData)
    {
        var reversedStructureFields = structureFields.AsEnumerable().Reverse();
        foreach (var field in reversedStructureFields)
        {
            var value = field.Value;
            gameModule.Fields.AddOrUpdate(field.Key, ref value);
        }

        foreach (var field in gameModuleBuildData.Fields)
        {
            var value = field.Value;
            gameModule.Fields.AddOrUpdate(field.Key, ref value);
        }

        gameModule.Fields.AddOrUpdate("Id", gameModule.Id);
        gameModule.Fields.AddOrUpdate("Type", gameModule.Type.ToString());
    }
}