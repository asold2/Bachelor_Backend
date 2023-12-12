using Models.UtilityCalculationRow;

public interface IFacade
{
    Task<List<UtilityCalculationRow>> GetAzureLogs();
    Task<List<UtilityZonesDateRow>> GetUtilitesWithZonesByDate();
    Task<int> GetNumberOfServicesDown();

    Task<IList<UtilityLatestCRMData>> GetUtilityLatestRTOData();

}