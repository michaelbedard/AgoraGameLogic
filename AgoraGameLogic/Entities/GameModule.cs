using AgoraGameLogic.Domain.Enums;
using Newtonsoft.Json;

namespace AgoraGameLogic.Domain.Entities.Models;

public class GameModule
{
    public string Id;
    public string Name;
    public GameModuleType Type;
    public string[] Structures;
    public Context Fields;

    public GameModule(string id, string name, GameModuleType type, string[] structures)
    {
        Id = id;
        Name = name;
        Type = type;
        Structures = structures;
        Fields = new Context();
    }

    public List<GameModule> GetChildren()
    {
        var result = new List<GameModule>();
        switch (Type)
        {
            case GameModuleType.Player:
            {
                result.AddRange(Fields.Get<List<GameModule>>("Hand"));
                
                var modules = Fields.Get<List<GameModule>>("Modules");
                foreach (var module in modules)
                {
                    result.AddRange(module.GetChildren());
                }
                
                break;
            }
            case GameModuleType.Deck:
            {
                result.AddRange(Fields.Get<List<GameModule>>("Cards"));
                break;
            }
            case GameModuleType.Zone:
            {
                result.AddRange(Fields.Get<List<GameModule>>("Cards"));
                break;
            }
        }

        return result;
    }
    
    public override string ToString()
    {
        var output = new
        {
            Name = Name,
            Type = Type,
            Fields = Fields.ToString()
        };

        // Serialize and print the output
        return JsonConvert.SerializeObject(output, Formatting.None).Replace("\"", "");
    }
}