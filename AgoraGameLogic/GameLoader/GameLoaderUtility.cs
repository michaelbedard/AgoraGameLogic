using System.Collections.Generic;
using AgoraGameLogic.Utility.BuildData;
using AgoraGameLogic.Utility.Enums;

namespace AgoraGameLogic.GameLoader;

public class GameLoaderUtility
{
    public static List<string> GetNamesForGameModuleOrThrow(string baseName, GameModuleBuildData gameModuleBuildData, int numberOfPlayers)
    {
        var names = new List<string>();
        switch (gameModuleBuildData.Type)
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
                for (var i = 0; i < gameModuleBuildData.Iterations; i++)
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

    public static List<StructureBuildData> GetStructureHierarchyOrThrow(StructureBuildData[] structureDefinitions, string structureName)
    {
        return GetStructureHierarchyHelper(structureDefinitions, structureName, new List<StructureBuildData>());
    }

    private static List<StructureBuildData> GetStructureHierarchyHelper(StructureBuildData[] structureDefinitions, string structureName, List<StructureBuildData> acc)
    {
        if (string.IsNullOrEmpty(structureName))
        {
            return new List<StructureBuildData>();
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