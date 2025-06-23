using ImporterFunction.Helpers;
using System.Text.Json.Serialization;

namespace ImporterFunction.Models;

public class ImportCategory
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    public string ETag => $"{Name}|{Description}".ToSha256Hash();
}