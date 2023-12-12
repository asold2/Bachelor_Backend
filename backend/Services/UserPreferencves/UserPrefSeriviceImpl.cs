using Castle.Core.Internal;
using Newtonsoft.Json;

public class UserPrefServiceImpl : IUserPrefService
{
    private readonly IUserPref _userPref;
    private readonly ILogger<UserPrefServiceImpl> _logger;
    private readonly string _fileName;
    private readonly string _filePath;

    public UserPrefServiceImpl(IUserPref userPref, ILogger<UserPrefServiceImpl> logger)
    {
        _userPref = userPref;
        _logger = logger;
        _fileName = "user_preferences.json";
        _filePath = Path.Combine(Environment.CurrentDirectory, _fileName);
        CreateJsonFile();

    }


    public async Task<int> AddUserPrefToFile(UserPref userPref)
    {
        if (userPref == null || userPref.UserId.IsNullOrEmpty())
        {
            _logger.LogInformation("User preference is null.");
            return 400;
        }
        IList<UserPref> existingPrefs = await _userPref.GetAllUserPrefsFromFile(_filePath);

        if (existingPrefs.Any(x => x.UserId == userPref.UserId))
        {
            var neededUserPref = existingPrefs.FirstOrDefault(x => x.UserId == userPref.UserId);
            existingPrefs.Remove(neededUserPref);
            existingPrefs.Add(userPref);
            string json2 = JsonConvert.SerializeObject(existingPrefs);
            await _userPref.AddUserPrefToFile(_filePath, json2);
            return 201;
        }

        existingPrefs.Add(userPref);

        string json = JsonConvert.SerializeObject(existingPrefs);

        await _userPref.AddUserPrefToFile(_filePath, json);
        _logger.LogInformation("Added user pref to file for user: " + userPref.UserId);
        return 200;

    }

    public void CreateJsonFile()
    {
        _logger.LogInformation("Creating json file");
        _userPref.CreateJsonFile(_filePath);
    }

    public async Task<int> DeleteUserPrefFromFile(string userId)
    {
        IList<UserPref> existingPrefs = await _userPref.GetAllUserPrefsFromFile(_filePath);

        if (userId == null || !existingPrefs.Any(x => x.UserId == userId))
        {
            _logger.LogInformation("User id is null or not found.");
            return 400;
        }

        UserPref userPref = existingPrefs.FirstOrDefault(x => x.UserId == userId);

        string json = "";
        if (userPref != null)
        {
            existingPrefs.Remove(userPref);
            json = JsonConvert.SerializeObject(existingPrefs);
        }
        await _userPref.DeleteUserPrefFromFile(_filePath, json);

        _logger.LogInformation("Deleted user pref from file fro user: " + userId + ".");
        return 200;

    }

    public async Task<UserPref> GetUserPrefFromFile(string userId)
    {

        IList<UserPref> existingPrefs = await _userPref.GetAllUserPrefsFromFile(_filePath);

        UserPref userPref = existingPrefs.FirstOrDefault(x => x.UserId == userId);

        if (userPref != null)
        {
            _logger.LogInformation("User preference retrieved from file for user: " + userId + ".");
            return userPref;
        }
        else
        {
            _logger.LogInformation("User preference not found in file for user: " + userId + ".");
            return null;
        }
    }

    public async Task<int> UpdateUserPrefInFile(UserPref userPref)
    {
        if (userPref == null || userPref.UserId.IsNullOrEmpty())
        {
            _logger.LogInformation("User preference is null.");
            return 400;
        }

        IList<UserPref> existingPrefs = await _userPref.GetAllUserPrefsFromFile(_filePath);

        if (!existingPrefs.Any(x => x.UserId == userPref.UserId))
        {
            _logger.LogInformation("User preference not found in file for user: " + userPref.UserId + ".");
            return 404;
        }

        UserPref existingUserPref = existingPrefs.FirstOrDefault(x => x.UserId == userPref.UserId);

        if (existingUserPref != null)
        {
            // existingUserPref = userPref;
            existingPrefs.Remove(existingUserPref);
            existingPrefs.Add(userPref);
            string json = JsonConvert.SerializeObject(existingPrefs);
            await _userPref.UpdateUserPrefInFile(_filePath, json);
            _logger.LogInformation("User preference updated in file fro user: " + userPref.UserId + ".");

        }
        return 200;
    }
}