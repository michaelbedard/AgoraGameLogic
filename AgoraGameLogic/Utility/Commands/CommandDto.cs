using Newtonsoft.Json;

namespace AgoraGameLogic.Domain.Entities.DataObject;

public class CommandDto
{
    [JsonProperty(Order = 3)]
    public int Id { get; set; }
    [JsonProperty(Order = 2)]
    public string Key { get; set; }
    [JsonProperty(Order = 1)]
    public string? TargetId { get; set; }
    [JsonProperty(Order = 0)]
    public Dictionary<string, object> Options { get; set; }
}