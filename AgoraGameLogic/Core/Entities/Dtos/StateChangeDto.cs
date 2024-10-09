using System.Text;
using Newtonsoft.Json;

namespace AgoraGameLogic.Domain.Entities.DataObject;

public class StateChangeDto
{
    public Dictionary<string, BaseCommandDto[]> Animations { get; set; }
    public Dictionary<string, BaseCommandDto[]> Actions { get; set; }
    public Dictionary<string, BaseCommandDto[]> Inputs { get; set; }
    public Dictionary<string, DescriptionDto[]> Descriptions { get; set; }

    public string ToString(bool showAnimations, bool showDescriptions)
    {
        var builder = new StringBuilder();

        builder.AppendLine("## StateChangeResult ##");


        foreach (var entry in Actions)
        {
            builder.AppendLine($"Commands for {entry.Key} : ");
            foreach (var command in entry.Value)
            {
                builder.AppendLine($"    Id: {command.Id}, Key: {command.Key}, Args: {JsonConvert.SerializeObject(command, Formatting.None)}, Options: {JsonConvert.SerializeObject(command.Options, Formatting.None)}");
            }

            builder.AppendLine("");
        }

        if (showAnimations)
        {
            foreach (var entry in Animations)
            {
                builder.AppendLine($"Animations for {entry.Key} : ");
                foreach (var animation in entry.Value)
                {
                    builder.AppendLine($"    Key: {animation.Key}, Args: {JsonConvert.SerializeObject(animation, Formatting.None)}, Options: {JsonConvert.SerializeObject(animation.Options, Formatting.None)}");
                }

                builder.AppendLine("");
            }
        }
        
        if (showDescriptions)
        {
            foreach (var entry in Descriptions)
            {
                builder.AppendLine($"Descriptions for {entry.Key} : ");
                foreach (var description in entry.Value)
                {
                    builder.AppendLine($"    Name: {description.Name}, Text: {description.Text}");
                }

                builder.AppendLine("");
            }
        }

        return builder.ToString();
    }
}