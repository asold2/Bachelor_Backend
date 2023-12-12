using System.Collections;
using Models.UtilityCalculationRow;

public interface IUtilityService
{
    Task<List<UtilityCalculationRow>> GetAzureLogs();
    Task<List<UtilityZonesDateRow>> GetUtilitesWithZonesByDate();
}