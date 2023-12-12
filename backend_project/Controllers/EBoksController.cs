namespace backend.Controllers;
using Microsoft.AspNetCore.Mvc;

// [Authorize]
[Route("api/")]

public class EboksController : Controller
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