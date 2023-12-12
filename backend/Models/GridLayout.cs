using System.Text.Json.Serialization;

public class GridLayout
{
    [JsonPropertyName("Tiles")]
    public IList<Tile> Tiles { get; set; }
}