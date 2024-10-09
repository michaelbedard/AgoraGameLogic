namespace AgoraGameLogic.Domain.Entities.DataObject.ActionCommandDtos;

public class DrawCardActionDto : BaseCommandDto
{
    public string PlayerId { get; set; }
    public string DeckId { get; set; }
}