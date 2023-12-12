using System.Text.Json.Serialization;

public class UtilityLatestCRMData
{
    [JsonPropertyName("RowId")]
    public int RowId { get; set; }
    [JsonPropertyName("CustomerId")]
    public string CustomerId { get; set; }
    [JsonPropertyName("UtilityName")]
    public string UtilityName { get; set; }
    [JsonPropertyName("LatestCRMData")]
    public DateTimeOffset LatestCRMData { get; set; }
    [JsonPropertyName("LatestCalculationDate")]
    public DateTimeOffset LatestCalculationDate { get; set; }
}