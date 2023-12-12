public interface IUserPrefService
{
    Task<int> AddUserPrefToFile(UserPref userPref);
    void CreateJsonFile();
    Task<int> DeleteUserPrefFromFile(string userId);
    Task<UserPref> GetUserPrefFromFile(string userId);
    Task<int> UpdateUserPrefInFile(UserPref userPref);
}