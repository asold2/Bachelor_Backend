public class AzureQueries
{
    public const string UTILITIESWITHLASTDATE = @"
                let CustomerName = traces
                | where message has ""FunctionLogCustomers: Active customer""
                | extend customerId = tostring(customDimensions.Guid), NameOfUtility = tostring(customDimensions.NameOfUtility);

                let CalculationStatus = traces
                | where message has ""CalculationStatusSnapshot""
                | extend customer = tostring(customDimensions.CustomerId), LastCalculatedDate = format_datetime(todatetime(tostring(customDimensions.LastCalculatedDate)), ""yyyy-MM-dd""), LastCompleteDate = format_datetime(todatetime(tostring(customDimensions.LastCompleteDate)), ""yyyy-MM-dd""), DatesMissingData = toint(customDimensions.CountMissingDates), AnyFailure = toboolean(customDimensions.AnyFailedCalculation), snapshot = todatetime(tostring(customDimensions.SnapshotTimestamp))
                | extend MissingDatesNullable = datetime_diff(""day"", now(-1d), todatetime(LastCompleteDate))
                | extend MissingDates = iff(isnull(MissingDatesNullable), 10, MissingDatesNullable)
                | summarize arg_max(snapshot, LastCalculatedDate, LastCompleteDate, MissingDates, DatesMissingData, AnyFailure) by customer;

                CustomerName
                | join kind=inner CalculationStatus on $left.customerId == $right.customer
                | project NameOfUtility, LastCalculatedDate, LastCompleteDate, MissingDates, DatesMissingData
                | order by MissingDates desc";

    public const string UTILITIESWITHZONESBYDATE = @"let CustomerName =
            traces
            | where message has ""FunctionLogCustomers: Active customer""
            | extend
                customerId = tostring(customDimensions.Guid),
                NameOfUtility = tostring(customDimensions.NameOfUtility);
        let CalculationStatus =
            traces
            | where message has ""LatestCalculationsSnapshot""
            | extend
                customer = tostring(customDimensions.CustomerId),
                CalculationDate = format_datetime(todatetime(tostring(customDimensions.CalculationDate)), ""yyyy-MM-dd""),
                snapshot = todatetime(tostring(customDimensions.SnapShotTimestamp))     
            | extend Zone1 = tostring(customDimensions.[""Zone 1""])
            | extend Zone2 = tostring(customDimensions.[""Zone 2""])
            | extend Zone3 = tostring(customDimensions.[""Zone 3""])
            | extend Zone4 = tostring(customDimensions.[""Zone 4""])
            | extend Zone5 = tostring(customDimensions.[""Zone 5""])
            | extend Zone6 = tostring(customDimensions.[""Zone 6""])
            | extend Zone7 = tostring(customDimensions.[""Zone 7""])
            | extend Zone8 = tostring(customDimensions.[""Zone 8""])
            | where todatetime(CalculationDate) < now(-1d)
            | summarize arg_max(snapshot, Zone1, Zone2, Zone3, Zone4, Zone5, Zone6, Zone7, Zone8) by CalculationDate, customer;
        CustomerName
        | join CalculationStatus on $left.customerId == $right.customer
        | project
            NameOfUtility,
            CalculationDate,
            Zone1,
            Zone2,
            Zone3,
            Zone4,
            Zone5,
            Zone6,
            Zone7,
            Zone8
        ";

    public const string LOGSCONSUMPTIONMETRICSFORINSTALLATION = @"
        traces
        | where customDimensions contains ""CalculateConsumptionMetricsForInstallationsJob""
        | where message  contains ""customer""
        | where message contains ""Loaded data""
            ";

    public const string LOGPULLEDDATAFROMCRMCUSTOMER = @"
        traces
            | where customDimensions contains ""PullInstallationsDataJob""
            | where message  contains ""pulled devices from""
            ";

    public const string UTILITYNAMESPART1 = @"
        traces
            | where message has ""FunctionLogCustomers: Active customer""
            | extend customerId = tostring(customDimensions.Guid), NameOfUtility = customDimensions.NameOfUtility
            | where customerId in 
        ";

    public const string UTILITYNAMESPART2 = @"
        | project customerId, NameOfUtility
        ";


}