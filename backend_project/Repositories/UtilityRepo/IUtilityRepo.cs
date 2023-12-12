using Azure;
using Azure.Monitor.Query.Models;

public interface IUtilityRepo
{
    public Task<Response<LogsQueryResult>> GetUtilities(string query);
    public Task<Response<LogsQueryResult>> GetUtilitiesWithZonesByDate(string query);

    public Task<Response<LogsQueryResult>> GetNamesOfUtilities(string query);
}