
using System.Text.RegularExpressions;
using Azure;
using Azure.Monitor.Query.Models;

public class RtoDataServiceImpl : IRtoDataService
{
    private readonly IRtoRepo _rtoRepo;
    private readonly IUtilityRepo _utilityRepo;
    private readonly ILogger<RtoDataServiceImpl> _logger;

    private readonly string queryCalculations = AzureQueries.LOGSCONSUMPTIONMETRICSFORINSTALLATION;
    private readonly string queryPulledCRMData = AzureQueries.LOGPULLEDDATAFROMCRMCUSTOMER;
    private readonly string query1 = AzureQueries.UTILITYNAMESPART1;
    private readonly string query2 = AzureQueries.UTILITYNAMESPART2;
    private readonly string pattern = @"\b[0-9a-f]{8}-([0-9a-f]{4}-){3}[0-9a-f]{12}\b";


    public RtoDataServiceImpl(IRtoRepo rtoRepo, IUtilityRepo utilityRepo, ILogger<RtoDataServiceImpl> logger)

    {
        _rtoRepo = rtoRepo;
        _utilityRepo = utilityRepo;
        _logger = logger;
    }

    public async Task<IList<UtilityLatestCRMData>> GetUtilityLatestRTOData()
    {
        //consumptionLogs refers to Calculations that are saved in the datastor in Step7
        //pulledLogs refers to the CRM data that is pulled from each customer in Step1

        IList<UtilityLatestCRMData> utilitiesAndCalculationStatus = new List<UtilityLatestCRMData>();

        var calculationsGroupedByCustomerId = await GetCalculationsGroup();
        var pulledDataGroupedByCustomerId = await GetPulledDataGroup();

        // Iterate through the keys common in both dictionaries
        var commonKeys = calculationsGroupedByCustomerId.Keys.Intersect(pulledDataGroupedByCustomerId.Keys);

        foreach (var key in commonKeys)
        {
            // Create UtilityLatestCRMData object
            var utilityData = new UtilityLatestCRMData
            {
                CustomerId = key,
                LatestCRMData = pulledDataGroupedByCustomerId[key],
                LatestCalculationDate = calculationsGroupedByCustomerId[key]
            };

            utilitiesAndCalculationStatus.Add(utilityData);
        }
        utilitiesAndCalculationStatus = await GetUtilitiesWithNames(utilitiesAndCalculationStatus);

        int count = 1;

        utilitiesAndCalculationStatus = utilitiesAndCalculationStatus
            .Where(utility => utility.UtilityName != null)
            .ToList();


        foreach (UtilityLatestCRMData utility in utilitiesAndCalculationStatus)
        {
            utility.RowId = count;
            count++;
        }

        _logger.LogInformation("Returning data with latest CRM data and latest calculation date");

        return utilitiesAndCalculationStatus;

    }


    public async Task<IList<UtilityLatestCRMData>> GetUtilitiesWithNames(IList<UtilityLatestCRMData> list)
    {
        string namesForQuery = "(";

        foreach (UtilityLatestCRMData utility in list)
        {
            namesForQuery += "'" + utility.CustomerId + "',";
        }
        namesForQuery = namesForQuery.Remove(namesForQuery.Length - 1);

        namesForQuery = namesForQuery + ")";

        string finalQuery = query1 + namesForQuery + query2;

        Response<LogsQueryResult> results = await _utilityRepo.GetNamesOfUtilities(finalQuery);

        LogsTable logs = results.Value.Table;

        foreach (LogsTableRow row in logs.Rows)
        {
            var utilityName = row["NameOfUtility"].ToString();
            var customerId = row["customerId"].ToString();

            foreach (UtilityLatestCRMData utility in list)
            {
                if (customerId == utility.CustomerId)
                {
                    utility.UtilityName = utilityName;
                }
            }
        }

        return list;
    }

    public async Task<Dictionary<string, DateTimeOffset>> GetCalculationsGroup()
    {
        Response<LogsQueryResult> results = await _rtoRepo.GetUtilityLatestCRMData(queryCalculations);
        LogsTable consumptionLogs = results.Value.Table;

        Dictionary<DateTimeOffset, string> dateCustomerLoadedData = new Dictionary<DateTimeOffset, string>();

        foreach (LogsTableRow row in consumptionLogs.Rows)
        {
            var timeStamp = DateTimeOffset.Parse(row["timestamp"].ToString());
            Match match = Regex.Match(row["message"].ToString(), pattern);
            string customerId = match.Value;

            dateCustomerLoadedData.Add(timeStamp, customerId);
        }


        //this is a dictionary that contains the latest date when data about a customer was loaded
        var calculationsGroupedByCustomerId = dateCustomerLoadedData
            .GroupBy(pair => pair.Value)
            .ToDictionary(
                group => group.Key,
                group => group.Max(item => item.Key)
            );

        return calculationsGroupedByCustomerId;
    }

    public async Task<Dictionary<string, DateTimeOffset>> GetPulledDataGroup()
    {
        Response<LogsQueryResult> pulledResults = await _rtoRepo.GetUtilityLatestCRMData(queryPulledCRMData);
        LogsTable pulledLogs = pulledResults.Value.Table;

        Dictionary<DateTimeOffset, string> pulledDataFromCustomer = new Dictionary<DateTimeOffset, string>();

        foreach (LogsTableRow row in pulledLogs.Rows)
        {
            var timeStamp = DateTimeOffset.Parse(row["timestamp"].ToString());
            Match match = Regex.Match(row["message"].ToString(), pattern);
            string customerId = match.Value;

            pulledDataFromCustomer.Add(timeStamp, customerId);
        }

        var pulledDataGroupedByCustomerId = pulledDataFromCustomer
            .GroupBy(pair => pair.Value)
            .ToDictionary(
                group => group.Key,
                group => group.Max(item => item.Key)
            );

        return pulledDataGroupedByCustomerId;
    }
}

