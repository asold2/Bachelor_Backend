using Newtonsoft.Json;
public class UserPrefImpl : IUserPref
{

    private readonly ILogger<UserPrefImpl> _logger;

    public UserPrefImpl(ILogger<UserPrefImpl> logger)
    {
        _logger = logger;
    }

    public UserPrefImpl() { }
    public void CreateJsonFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
    }

    public async Task AddUserPrefToFile(string _filePath, string json)
    {
        try
        {
            File.WriteAllText(_filePath, json);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }

    public async Task DeleteUserPrefFromFile(string _filePath, string json)
    {
        try
        {
            File.WriteAllText(_filePath, json);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }



    public async Task UpdateUserPrefInFile(string _filePath, string json)
    {
        try
        {
            File.WriteAllText(_filePath, json);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }

    public async Task<IList<UserPref>> GetAllUserPrefsFromFile(string _filePath)
    {
        try
        {
            string json = await File.ReadAllTextAsync(_filePath);
            IList<UserPref> existingPrefs = JsonConvert.DeserializeObject<IList<UserPref>>(json) ?? new List<UserPref>();
            return existingPrefs;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return new List<UserPref>();
        }
    }


}