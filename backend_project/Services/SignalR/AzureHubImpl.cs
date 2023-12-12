namespace backend.Services.SignalR;

using System.Threading;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

// [Authorize]
[EnableCors]
public class AzureHubImpl : Hub, IHostedService
{

    private readonly ILogger<AzureHubImpl> _logger;
    private int connectedClientsCount = 0;
    private Timer? _timer;
    private readonly IHubContext<AzureHubImpl> _hubContext;


    public AzureHubImpl(ILogger<AzureHubImpl> logger, IHubContext<AzureHubImpl> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine("OnConnectedAsync called.");
        // showing that client is connected

        connectedClientsCount++;
        _logger.LogInformation($"Client connected. Total connected clients: {connectedClientsCount}");

        await base.OnConnectedAsync();

        if (DataHolderSingleton.Instance.LastDataCollection.DateTime == default)
        {
            await _hubContext.Clients.All.SendAsync("LastDataCollection", "No data has been collected yet.");
        }
        else
        {
            var test = _hubContext.Clients.All;
            await _hubContext.Clients.Client(Context.ConnectionId).SendAsync("LastDataCollection", DataHolderSingleton.Instance.LastDataCollection);
        }

        //sending data to client on new connection
        if (!string.IsNullOrEmpty(DataHolderSingleton.Instance.UtilityData))
        {
            await _hubContext.Clients.Client(Context.ConnectionId).SendAsync("UtilityData", DataHolderSingleton.Instance.UtilityData);
            _logger.LogInformation("Latest Utility Data sent to all connected clients.");
        }

        if (!string.IsNullOrEmpty(DataHolderSingleton.Instance.UtilitiesWithZonesByDate))
        {
            await _hubContext.Clients.Client(Context.ConnectionId).SendAsync("UtilityWithZones", DataHolderSingleton.Instance.UtilitiesWithZonesByDate);
            _logger.LogInformation("Latest Utility Data sent to all connected clients.");
        }

        if (!string.IsNullOrEmpty(DataHolderSingleton.Instance.LatestRTOData))
        {
            await _hubContext.Clients.Client(Context.ConnectionId).SendAsync("LatestRTOData", DataHolderSingleton.Instance.LatestRTOData);
            _logger.LogInformation("Latest RTO Data sent to all connected clients.");
        }

        if (!string.IsNullOrEmpty(DataHolderSingleton.Instance.EboksData))
        {
            await _hubContext.Clients.Client(Context.ConnectionId).SendAsync("EboksData", DataHolderSingleton.Instance.EboksData);
            _logger.LogInformation("Latest Eboks Data sent to all connected clients.");
        }


        _logger.LogInformation($"Sending data to newly connected client.");

    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        _logger.LogInformation($"Client {Context.ConnectionId} disconnected.");
        connectedClientsCount--;
        await base.OnDisconnectedAsync(exception);
    }

    public int GetNumberOfConnectedClients()
    {
        return connectedClientsCount;
    }

    public async Task Send()
    {
        if (_hubContext.Clients != null)
        {
            if (DataHolderSingleton.Instance.LastDataCollection.DateTime == default)
            {
                await _hubContext.Clients.All.SendAsync("LastDataCollection", "No data has been collected yet.");
            }
            else
            {
                await _hubContext.Clients.All.SendAsync("LastDataCollection", DataHolderSingleton.Instance.LastDataCollection);
            }

            //sending data to client on new connection
            if (!string.IsNullOrEmpty(DataHolderSingleton.Instance.UtilityData))
            {
                await _hubContext.Clients.All.SendAsync("UtilityData", DataHolderSingleton.Instance.UtilityData);
                _logger.LogInformation("Latest Utility Data sent to all connected clients.");
            }

            if (!string.IsNullOrEmpty(DataHolderSingleton.Instance.UtilitiesWithZonesByDate))
            {
                await _hubContext.Clients.All.SendAsync("UtilityWithZones", DataHolderSingleton.Instance.UtilitiesWithZonesByDate);
                _logger.LogInformation("Latest Utility Data sent to all connected clients.");
            }

            if (!string.IsNullOrEmpty(DataHolderSingleton.Instance.LatestRTOData))
            {
                await _hubContext.Clients.All.SendAsync("LatestRTOData", DataHolderSingleton.Instance.LatestRTOData);
                _logger.LogInformation("Latest RTO Data sent to all connected clients.");
            }

            if (!string.IsNullOrEmpty(DataHolderSingleton.Instance.EboksData))
            {
                await _hubContext.Clients.All.SendAsync("EboksData", DataHolderSingleton.Instance.EboksData);
                _logger.LogInformation("Latest Eboks Data sent to all connected clients.");
            }


            _logger.LogInformation($"Sending data to all connected clients. Total connected clients: {connectedClientsCount}");
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var semaphore = new SemaphoreSlim(1, 1); // Initialize a semaphore with a count of 1

        _timer = new Timer(async state =>
        {
            try
            {
                await semaphore.WaitAsync(); // Acquire the semaphore

                // Perform the action while holding the semaphore lock
                _logger.LogInformation("Sending data to all connected clients.");
                await Send();
            }
            finally
            {
                semaphore.Release(); // Release the semaphore lock
            }
        }, null, TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(5));

        _logger.LogInformation("Prepared to send data.");

    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }


}