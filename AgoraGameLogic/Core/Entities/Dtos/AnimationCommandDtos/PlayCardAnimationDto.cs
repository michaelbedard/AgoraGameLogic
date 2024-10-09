namespace AgoraGameLogic.Domain.Entities.DataObject.AnimationCommandDtos;

public class PlayCardAnimationDto : BaseCommandDto
{
    public string PlayerId { get; set; }
    public string CardId { get; set; }
}