using Azure.Identity;
using Azure.Monitor.Query;
using Azure.Monitor.Query.Models;
using Azure.Core;
using Models.UtilityCalculationRow;

using System.Data;
using Azure;

public class UtilityServiceImpl : IUtilityService
{
    private readonly ILogger<UtilityServiceImpl> _logger;
    private readonly IUtilityRepo _utilityRepo;
    private readonly string utilitiesWithDateQuery = AzureQueries.UTILITIESWITHLASTDATE;
    private readonly string utilitiesWithZonesQuery = AzureQueries.UTILITIESWITHZONESBYDATE;



    public UtilityServiceImpl(IUtilityRepo utilityRepo, ILogger<UtilityServiceImpl> logger)
    {
        _utilityRepo = utilityRepo;
        _logger = logger;
    }

    public async Task<List<UtilityCalculationRow>> GetAzureLogs()
    {
        int utilityId = 1;

        Response<LogsQueryResult> results = await _utilityRepo.GetUtilities(utilitiesWithDateQuery);

        LogsTable resultTable = results.Value.Table;

        List<UtilityCalculationRow> utilitiesAndCalculationStatus = new List<UtilityCalculationRow>();

        foreach (LogsTableRow row in resultTable.Rows)
        {
            var hasCalculation = false;
            // Access the data in each row
            string utility = row["NameOfUtility"].ToString();
            string lastCalculatedDate = row["LastCalculatedDate"].ToString();

            if (lastCalculatedDate == "" || lastCalculatedDate == null)
            {
                lastCalculatedDate = "1900-01-01";
            }

            DateTime datetime = DateTime.Parse(lastCalculatedDate);

            if (datetime >= DateTime.Now.AddDays(-1))
            {
                hasCalculation = true;
            }

            UtilityCalculationRow utilityCalculationRow = new UtilityCalculationRow
            {
                RowId = utilityId++,
                UtilityName = utility,
                CalculationLast24Hours = hasCalculation,
                // CalculationLast24Hours = lastCalculatedDate, 
            };

            utilitiesAndCalculationStatus.Add(utilityCalculationRow);
        }

        utilitiesAndCalculationStatus = utilitiesAndCalculationStatus.OrderBy(row => row.CalculationLast24Hours).ToList();
        _logger.LogInformation("Returning ordered utilities and calculation status");
        return utilitiesAndCalculationStatus;

    }

    public async Task<List<UtilityZonesDateRow>> GetUtilitesWithZonesByDate()
    {
        int rowId = 1;
        Response<LogsQueryResult> results = await _utilityRepo.GetUtilitiesWithZonesByDate(utilitiesWithZonesQuery);

        LogsTable resultTable = results.Value.Table;
        List<UtilityZonesDateRow> utilitiesWithzZonesByDate = new List<UtilityZonesDateRow>();

        foreach (LogsTableRow row in resultTable.Rows)
        {
            string utility = row["NameOfUtility"].ToString();
            string calculationDate = row["CalculationDate"].ToString();
            List<int> numberedZones = new List<int>();

            for (int i = 1; i <= 8; i++)
            {
                int missingPeriodsPerZone = 0;
                string zone = row["Zone" + i].ToString();

                if (zone == "" || zone == null)
                {
                    //-1 will mean that the zone didn't have any calculations
                    numberedZones.Add(-1);
                }

                else
                {
                    string[] elements = zone.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string element in elements)
                    {
                        if (element.Contains("NoCalculation"))
                        {
                            missingPeriodsPerZone++;
                        }
                    }
                    numberedZones.Add(missingPeriodsPerZone);
                }

                // numberedZones.Add(missingPeriodsPerZone);
            }
            UtilityZonesDateRow utilityZonesDateRow = new UtilityZonesDateRow
            {
                RowId = rowId++,
                UtilityName = utility,
                CalculationDate = DateTime.Parse(calculationDate),
                ZonePeriodsWithMissingCalculations = numberedZones
            };

            utilitiesWithzZonesByDate.Add(utilityZonesDateRow);


        }
        _logger.LogInformation("Returning utilities with zones by date");
        utilitiesWithzZonesByDate = utilitiesWithzZonesByDate.OrderBy(row => row.RowId).ToList();
        return utilitiesWithzZonesByDate;

    }

}
