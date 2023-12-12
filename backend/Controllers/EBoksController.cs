namespace backend.Controllers;

using Kamstrup.Analytics.Security.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/")]

public class EboksController : BaseController
{

    private readonly IFacade _facade;
    private readonly ILogger<EboksController> _logger;

    public EboksController(IFacade facade, ILogger<EboksController> logger)
    {
        _facade = facade;
        _logger = logger;
    }

    [HttpGet("eboks")]
    public async Task<int> GetNumberOfDownEboksServices()
    {
        _logger.LogInformation("Getting number of down eboks services through REST API");
        return await _facade.GetNumberOfServicesDown();
    }



}