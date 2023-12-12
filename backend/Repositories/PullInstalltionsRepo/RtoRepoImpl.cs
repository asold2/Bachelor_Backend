
using Azure.Identity;
using Azure.Monitor.Query;
using Azure.Monitor.Query.Models;
using Azure;
using Azure.Core;

public class RtoRepoImpl : IRtoRepo
{
    public RtoRepoImpl()
    {
    }

    public async Task<Response<LogsQueryResult>> GetUtilityLatestCRMData(string query)
    {
        string resourceId = "/subscriptions/4ec53337-2e65-4a81-9017-bdd960af28aa/resourceGroups/rg-prod-analytics/providers/microsoft.insights/components/ai-prod-analytics";

        var client = new LogsQueryClient(new DefaultAzureCredential());


        Response<LogsQueryResult> results = await client.QueryResourceAsync(
            new ResourceIdentifier(resourceId),
            query,
            new QueryTimeRange(TimeSpan.FromDays(2)));

        return results;
    }
}