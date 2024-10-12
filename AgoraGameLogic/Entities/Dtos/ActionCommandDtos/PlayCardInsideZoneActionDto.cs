namespace AgoraGameLogic.Domain.Entities.DataObject.ActionCommandDtos;

public class PlayCardInsideZoneActionDto : BaseCommandDto
{
    public string CardId { get; set; }
    public string ZoneId { get; set; }
}