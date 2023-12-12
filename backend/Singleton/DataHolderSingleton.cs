public sealed class DataHolderSingleton
{
    private static readonly object padlock = new object();
    private static DataHolderSingleton _instance = null;

    private DataHolderSingleton() { }

    public static DataHolderSingleton Instance
    {
        get
        {
            lock (padlock)
            {
                if (_instance == null)
                {
                    _instance = new DataHolderSingleton();
                }
                return _instance;
            }
        }
    }

    public string UtilityData { get; set; }
    public string UtilitiesWithZonesByDate { get; set; }
    public string LatestRTOData { get; set; }
    public string EboksData { get; set; }
    public DateTimeOffset LastDataCollection { get; set; }
}
