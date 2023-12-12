using System.Text.Json;
using System.Text.Json.Serialization;

namespace Models.UtilityCalculationRow
{
    public class UtilityCalculationRow
    {
        [JsonPropertyName("RowId")]
        public int RowId { get; set; }
        [JsonPropertyName("UtilityName")]
        public string? UtilityName { get; set; }
        [JsonPropertyName("CalculationLast24Hours")]
        public bool? CalculationLast24Hours { get; set; }
    }
}