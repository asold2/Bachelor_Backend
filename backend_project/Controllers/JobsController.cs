using Microsoft.AspNetCore.Mvc;

// [Authorize]
[Route("api/")]
public class JobsController : Controller
{

    private readonly IFacade _facade;
    private readonly ILogger<JobsController> _logger;

    public JobsController(IFacade facade, ILogger<JobsController> logger)
    {
        _facade = facade;
        _logger = logger;
    }

    // [AuthorizedToken]
    [HttpGet("jobs")]
    public async Task<IList<UtilityLatestCRMData>> GetJobs()
    {
        _logger.LogInformation("Getting jobs through REST API");
        return await _facade.GetUtilityLatestRTOData();
    }

}
