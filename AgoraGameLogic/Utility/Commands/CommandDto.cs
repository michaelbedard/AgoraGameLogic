namespace AgoraGameLogic.Domain.Entities.DataObject;

public class CommandDto
{
    public int Id { get; set; }
    public string Key { get; set; }
    public string? TargetId { get; set; }
    public Dictionary<string, object> Options { get; set; }
}