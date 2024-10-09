using AgoraGameLogic.Domain.Entities.DataObject;

namespace AgoraGameLogic.Logic.Blocks.Inputs;

public class ChoiceInputDto : BaseCommandDto
{
    public string PlayerId { get; set; }
    public object[] Choices { get; set; }
}