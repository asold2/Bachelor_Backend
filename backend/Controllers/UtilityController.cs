using Kamstrup.Analytics.Security.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.UtilityCalculationRow;

[Authorize]
[Route("api/")]

public class UtilityController : BaseController
{

    private readonly IFacade _facade;
    private readonly ILogger<UtilityController> _logger;

    public UtilityController(IFacade facade, ILogger<UtilityController> logger)
    {
        _facade = facade;
        _logger = logger;
    }

    // [AuthorizedToken]
    [HttpGet("utilities")]
    public async Task<List<UtilityCalculationRow>> GetUtilities()
    {
        _logger.LogInformation("Getting utilities through REST API");
        return await _facade.GetAzureLogs();
    }

    // [AuthorizedToken]
    [HttpGet("utilities/zones")]
    public async Task<List<UtilityZonesDateRow>> GetUtilitiesWithZones()
    {
        _logger.LogInformation("Getting utilities with zones through REST API");
        return await _facade.GetUtilitesWithZonesByDate();
    }


}