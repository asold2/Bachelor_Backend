using Microsoft.AspNetCore.Mvc;

// [Authorize]
public class UserPrefController : Controller
{

    private readonly IUserPrefService _userPrefService;
    private readonly ILogger<UserPrefController> _logger;

    public UserPrefController(IUserPrefService userPrefService, ILogger<UserPrefController> logger)
    {
        _userPrefService = userPrefService;
        _logger = logger;
    }

    // [AuthorizedToken]
    [HttpGet("userpref")]
    public async Task<ActionResult<UserPref>> GetUserPref([FromQuery] string userId)
    {
        _logger.LogInformation("Getting user pref through REST API");
        var result = await _userPrefService.GetUserPrefFromFile(userId);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // [AuthorizedToken]
    [HttpPost("userpref")]
    public async Task<ActionResult> AddUserPref([FromBody] UserPref userPref)
    {
        _logger.LogInformation("Adding user pref through REST API");
        var result = await _userPrefService.AddUserPrefToFile(userPref);
        return new StatusCodeResult(result);
    }

    // [AuthorizedToken]
    // [HttpPut("userpref")]
    public async Task<ActionResult> UpdateUserPref([FromBody] UserPref userPref)
    {
        _logger.LogInformation("Updating user pref through REST API");
        var result = await _userPrefService.UpdateUserPrefInFile(userPref);
        return new StatusCodeResult(result);
    }

    // [AuthorizedToken]
    [HttpDelete("userpref")]
    public async Task<ActionResult> DeleteUserPref([FromQuery] string userId)
    {
        _logger.LogInformation("Deleting user pref through REST API");
        var result = await _userPrefService.DeleteUserPrefFromFile(userId);
        return new StatusCodeResult(result);
    }
}