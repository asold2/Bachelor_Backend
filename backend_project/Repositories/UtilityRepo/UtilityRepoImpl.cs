
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Monitor.Query;
using Azure.Monitor.Query.Models;

public class UtilityRepoImpl : IUtilityRepo
{
    LogsQueryClient client;

    public UtilityRepoImpl()
    {
        client = new LogsQueryClient(new DefaultAzureCredential());
    }

    public async Task<Response<LogsQueryResult>> GetNamesOfUtilities(string query)
    {
        string resourceId = "/subscriptions/4ec53337-2e65-4a81-9017-bdd960af28aa/resourceGroups/rg-prod-analytics/providers/microsoft.insights/components/ai-prod-analytics";

        Response<LogsQueryResult> results = await client.QueryResourceAsync(
            new ResourceIdentifier(resourceId),
            query
            , new QueryTimeRange(TimeSpan.FromDays(2))
            );

        return results;
    }

    public async Task<Response<LogsQueryResult>> GetUtilities(string query)
    {
        string resourceId = "/subscriptions/4ec53337-2e65-4a81-9017-bdd960af28aa/resourceGroups/rg-prod-analytics/providers/microsoft.insights/components/ai-prod-analytics";

        Response<LogsQueryResult> results = await client.QueryResourceAsync(
            new ResourceIdentifier(resourceId),
            query,
            new QueryTimeRange(TimeSpan.FromDays(2)));

        return results;
    }

    public async Task<Response<LogsQueryResult>> GetUtilitiesWithZonesByDate(string query)
    {
        string resourceId = "/subscriptions/4ec53337-2e65-4a81-9017-bdd960af28aa/resourceGroups/rg-prod-analytics/providers/microsoft.insights/components/ai-prod-analytics";


        Response<LogsQueryResult> results = await client.QueryResourceAsync(
            new ResourceIdentifier(resourceId),
            query,
            new QueryTimeRange(TimeSpan.FromDays(2)));

        return results;
    }
}