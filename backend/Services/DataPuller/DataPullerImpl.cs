using System.Text.Json;


public class DataPullerImpl : IHostedService
{

    private readonly IFacade _facade;
    private readonly ILogger<DataPullerImpl> _logger;

    private Timer? _timer;

    public DataPullerImpl(IFacade facade
    , ILogger<DataPullerImpl> logger)
    {
        _facade = facade;
        _logger = logger;
    }

    public async Task StoreDataFromSources()
    {
        _logger.LogInformation("Storing data from sources");
        var data = await _facade.GetAzureLogs();
        var utilitiesWithZonesByDate = await _facade.GetUtilitesWithZonesByDate();
        var latestRTOData = await _facade.GetUtilityLatestRTOData();
        var numberEboksServicesDown = await _facade.GetNumberOfServicesDown();

        var jsonData = JsonSerializer.Serialize(data);
        var jsonDataWithZones = JsonSerializer.Serialize(utilitiesWithZonesByDate);
        var jsonLatestRTOData = JsonSerializer.Serialize(latestRTOData);
        var jsonNumberEboksDownServies = JsonSerializer.Serialize(numberEboksServicesDown);

        DataHolderSingleton.Instance.UtilityData = jsonData;
        _logger.LogInformation("Utility Data Received");

        DataHolderSingleton.Instance.UtilitiesWithZonesByDate = jsonDataWithZones;
        _logger.LogInformation("Utility Data With Zones Received");


        DataHolderSingleton.Instance.LatestRTOData = jsonLatestRTOData;
        _logger.LogInformation("RTO Data Received");

        DataHolderSingleton.Instance.EboksData = jsonNumberEboksDownServies;
        _logger.LogInformation("Number of Eboks Services Down Received");

        DataHolderSingleton.Instance.LastDataCollection = DateTimeOffset.Now;
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(async state => await StoreDataFromSources(), null, TimeSpan.FromSeconds(10), TimeSpan.FromHours(1));
        _logger.LogInformation("Timer started with interval of 1 hour");

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }
}

