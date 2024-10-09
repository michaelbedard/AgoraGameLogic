using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Enums;
using AgoraGameLogic.Domain.Extensions;
using Newtonsoft.Json.Linq;

namespace AgoraGameLogic.Control.GameLoader;

public partial class GameLoader
{
    private List<GameModule> BuildGameModule(Dictionary<GameModule, GameModuleDefinition> gameModulesToDefinition, GameModuleDefinition gameModuleDefinition, StructureDefinition[] structureDefinitions, GameData gameData, GameModule? parent = null)
    {
        var structureHierarchy = GameLoaderUtility.GetStructureHierarchy(structureDefinitions, gameModuleDefinition.Structure);
        var structureHierarchyNames = structureHierarchy.Select(s => s.Name).ToArray();
        
        // get fields from hierarchy
        var structureFields = new List<KeyValuePairDefinition>();
        if (structureHierarchy.Count > 0)
        {
            foreach (var structure in structureHierarchy)
            {
                structureFields.AddRange(structure.Fields);
            }
        }
        
        // extend name, if necessary
        var baseName = gameModuleDefinition.Name;
        var extendedName = baseName;
        if (parent != null)
        {
            extendedName = $"{parent.Name}_{baseName}";
        }
        
        // add it to the context
        var names = GameLoaderUtility.GetNamesForGameModule(extendedName, gameModuleDefinition, gameData.Players.Count());
        var buildGameModules = new List<GameModule>();
        foreach (var name in names)
        {
            var gameModule = new GameModule(name, baseName, gameModuleDefinition.Type, structureHierarchyNames);
            buildGameModules.Add(gameModule);

            // add to context if global
            if (parent == null)
            {
                gameData.GlobalContext.AddOrUpdate(name, ref gameModule);
            }
            
            // add children, and create field reference
            switch (gameModule.Type)
            {
                case GameModuleType.Player:
                {
                    var modules = new List<GameModule>();
                    foreach (var playerModule in gameModuleDefinition.Modules)
                    {
                        var result = BuildGameModule(gameModulesToDefinition, playerModule, structureDefinitions, gameData, gameModule);
                        
                        // assume player modules are only 1-instance module
                        var player = result[0];
                        
                        modules.Add(player);
                        gameModulesToDefinition[player] = playerModule;
                        gameModule.Fields.AddOrUpdate(gameModuleDefinition.Name, ref player);
                    }

                    var hand = new List<GameModule>();
                    
                    gameModule.Fields.AddOrUpdate("Modules", ref modules);
                    gameModule.Fields.AddOrUpdate("Hand", ref hand);
                    
                    break;
                }
                case GameModuleType.Deck:
                {
                    var cards = new List<GameModule>();
                    foreach (var cardModule in gameModuleDefinition.Cards)
                    {
                        var result = BuildGameModule(gameModulesToDefinition, cardModule, structureDefinitions, gameData, gameModule);
                        foreach (var r in result)
                        {
                            cards.Add(r);
                            gameModulesToDefinition[r] = cardModule;
                        }
                    }

                    var numberOfCards = cards.Count;
                    
                    gameModule.Fields.AddOrUpdate("Cards", ref cards);
                    gameModule.Fields.AddOrUpdate("NumberOfCards", ref numberOfCards);
                    break;
                }
                case GameModuleType.Zone:
                {
                    var cards = new List<GameModule>();
                    var numberOfCards = cards.Count();
                    
                    gameModule.Fields.AddOrUpdate("Cards", ref cards);
                    gameModule.Fields.AddOrUpdate("NumberOfCards", ref numberOfCards);
                    break;
                }
            }

            // add gameModuleStructureFields.  Reverse because we want to add older fields first so that they are overwritten
            var temp = structureFields.ToList();
            temp.Reverse();
            foreach (var field in temp)
            {
                var value = field.Value;
                gameModule.Fields.AddOrUpdate(field.Key, ref value);
            }
            
            // add gameModuleFields (after so that it overrides)
            foreach (var field in gameModuleDefinition.Fields)
            {
                var value = field.Value;
                gameModule.Fields.AddOrUpdate(field.Key, ref value);
            }
        }

        return buildGameModules;
    }
}