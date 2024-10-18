namespace AgoraGameLogic.Dtos.ActionCommandDtos;

public class PlayCardInsideZoneActionDto : CommandDto
{
    public string CardId { get; set; }
    public string ZoneId { get; set; }
}