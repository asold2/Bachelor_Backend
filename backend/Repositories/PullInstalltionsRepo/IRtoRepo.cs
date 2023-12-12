using Azure;
using Azure.Monitor.Query.Models;

public interface IRtoRepo
{
    Task<Response<LogsQueryResult>> GetUtilityLatestCRMData(string query);
}