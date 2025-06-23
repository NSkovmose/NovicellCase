namespace ImporterFunction.Models;

using ImporterFunction.Helpers;
using System.Text.Json.Serialization;

public class ImportProduct
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("image")]
    public string Image { get; set; }

    public string ETag => $"{Title}|{Price}|{Description}|{Image}|{Category}".ToSha256Hash();
}
