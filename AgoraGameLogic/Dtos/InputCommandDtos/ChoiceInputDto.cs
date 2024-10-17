using AgoraGameLogic.Domain.Entities.DataObject;

namespace AgoraGameLogic.Logic.Blocks.Inputs;

public class ChoiceInputDto : CommandDto
{
    public object[] Choices { get; set; }
}