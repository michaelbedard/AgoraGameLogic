using AgoraGameLogic.Domain.Entities.DataObject;

namespace AgoraGameLogic.Logic.Blocks.Inputs;

public class ChoiceInputDto : BaseCommandDto
{
    public object[] Choices { get; set; }
}