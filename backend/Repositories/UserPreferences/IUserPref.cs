public interface IUserPref
{
    void CreateJsonFile(string filePath);
    Task AddUserPrefToFile(string _filePath, string json);
    Task UpdateUserPrefInFile(string _filePath, string json);
    Task DeleteUserPrefFromFile(string _filePath, string json);
    Task<IList<UserPref>> GetAllUserPrefsFromFile(string _filePath);
}