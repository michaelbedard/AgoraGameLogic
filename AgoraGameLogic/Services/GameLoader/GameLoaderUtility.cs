using AgoraGameLogic.Domain.Entities.BuildDefinition;
using AgoraGameLogic.Domain.Enums;

namespace AgoraGameLogic.Control.GameLoader;

public class GameLoaderUtility
{
    public static List<string> GetNamesForGameModule(string baseName, GameModuleDefinition gameModuleDefinition, int numberOfPlayers)
    {
        var names = new List<string>();
        switch (gameModuleDefinition.Type)
        {
            case GameModuleType.Player:
            {
                for (var i = 0; i < numberOfPlayers; i++)
                {
                    names.Add($"{baseName}{i}");
                }
                break;
            }
            case GameModuleType.Card:
            {
                for (var i = 0; i < gameModuleDefinition.Iterations; i++)
                {
                    names.Add($"{baseName}{i}");
                }
                
                break;
            }
            default:
            {
                names.Add(baseName);
                break;
            }
        }
        return names;
    }

    public static List<StructureDefinition> GetStructureHierarchy(StructureDefinition[] structureDefinitions, string structureName)
    {
        return GetStructureHierarchyHelper(structureDefinitions, structureName, new List<StructureDefinition>());
    }

    private static List<StructureDefinition> GetStructureHierarchyHelper(StructureDefinition[] structureDefinitions, string structureName, List<StructureDefinition> acc)
    {
        if (string.IsNullOrEmpty(structureName))
        {
            return new List<StructureDefinition>();
        }
        
        foreach (var structureDefinition in structureDefinitions)
        {
            if (structureDefinition.Name == structureName)
            {
                // found
                acc.Add(structureDefinition);
               
                if (string.IsNullOrEmpty(structureDefinition.Extension))
                {
                    // we reached the end
                    return acc;
                }
                else
                {
                    return GetStructureHierarchyHelper(structureDefinitions, structureDefinition.Extension, acc);
                }
            }
        }

        return acc;
    }
}