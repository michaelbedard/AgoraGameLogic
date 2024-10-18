using System.Collections.Generic;
using System.Text;
using AgoraGameLogic.Utility;
using Newtonsoft.Json;

namespace AgoraGameLogic.Dtos;

public class StateChangeDto
{
    public Dictionary<string, CommandDto[]> Animations { get; set; }
    public Dictionary<string, CommandDto[]> Actions { get; set; }
    public Dictionary<string, CommandDto[]> Inputs { get; set; }
    public Dictionary<string, DescriptionDto[]> Descriptions { get; set; }

    public string ToString(bool showAnimations, bool showDescriptions)
    {
        var builder = new StringBuilder();
        
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new ReversePropertyOrderContractResolver(),
            Formatting = Formatting.None
        };

        builder.AppendLine(">> Actions");
        foreach (var entry in Actions)
        {
            if (entry.Value.Length > 0)
            {
                builder.AppendLine($"Actions for {entry.Key} : ");
                foreach (var command in entry.Value)
                {
                    builder.AppendLine($"    {JsonConvert.SerializeObject(command, settings)}");
                }
            }
        }
        
        builder.AppendLine(">> Inputs");
        foreach (var entry in Inputs)
        {
            if (entry.Value.Length > 0)
            {
                builder.AppendLine($"Inputs for {entry.Key} : ");
                foreach (var command in entry.Value)
                {
                    builder.AppendLine($"    {JsonConvert.SerializeObject(command, settings)}");
                }
            }
        }

        if (showAnimations)
        {
            builder.AppendLine(">> Animations");
            foreach (var entry in Animations)
            {
                if (entry.Value.Length > 0)
                {
                    builder.AppendLine($"Animations for {entry.Key} : ");
                    foreach (var animation in entry.Value)
                    {
                        builder.AppendLine($"    {animation.ToString()}");
                    }
                }
            }
        }
        
        if (showDescriptions)
        {
            builder.AppendLine(">> Descriptions");
            foreach (var entry in Descriptions)
            {
                if (entry.Value.Length > 0)
                {
                    builder.AppendLine($"Descriptions for {entry.Key} : ");
                    foreach (var description in entry.Value)
                    {
                        builder.AppendLine($"    {description.ToString()}");
                    }
                }
            }
        }

        return builder.ToString();
    }
}