using System.Collections;
using System.Text.Json.Serialization;

namespace Models.UtilityCalculationRow
{
    public class UtilityZonesDateRow
    {
        [JsonPropertyName("RowId")]
        public int RowId { get; set; }
        [JsonPropertyName("UtilityName")]
        public string? UtilityName { get; set; }
        [JsonPropertyName("CalculationDate")]
        public DateTime? CalculationDate { get; set; }
        [JsonPropertyName("ZonePeriodsWithMissingCalculations")]
        public IList<int>? ZonePeriodsWithMissingCalculations { get; set; }

    }
}