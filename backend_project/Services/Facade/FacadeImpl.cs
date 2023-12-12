using Models.UtilityCalculationRow;

public class FacadeImpl : IFacade
{

    private readonly IUtilityService _utilityService;
    private readonly IEboksService _eboksService;
    private readonly IRtoDataService _rtoDataService;


    public FacadeImpl(
        IUtilityService utilityService,
        IEboksService eboksService,
        IRtoDataService rtoDataService
        )
    {
        _utilityService = utilityService;
        _eboksService = eboksService;
        _rtoDataService = rtoDataService;
    }


    public async Task<List<UtilityCalculationRow>> GetAzureLogs()
    {
        return await _utilityService.GetAzureLogs();
    }

    public async Task<List<UtilityZonesDateRow>> GetUtilitesWithZonesByDate()
    {
        return await _utilityService.GetUtilitesWithZonesByDate();
    }

    public async Task<int> GetNumberOfServicesDown()
    {
        return await _eboksService.GetNumberOfServicesDown();
    }

    public async Task<IList<UtilityLatestCRMData>> GetUtilityLatestRTOData()
    {
        return await _rtoDataService.GetUtilityLatestRTOData();
    }
}