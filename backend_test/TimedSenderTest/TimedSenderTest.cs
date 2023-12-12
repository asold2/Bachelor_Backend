namespace backend_test.TimedSenderTest;

using Microsoft.Extensions.Logging;


[TestClass]
public class TimedSenderTest{

    private static DataPullerImpl _timedSender;
    private static IFacade _facade;

    private static IUtilityService _utilityService;
    private static IEboksService _eboksService;
    private static IRtoDataService _rtoDataService;
    
    public TimedSenderTest(){}

    
    [ClassInitialize]
    public static void InitializeTestClass(TestContext context)
    {
        IntitializeUtilityLists().Wait(); // Wait for the async method to complete
    }

    /*This method initializes all the services that are used by the facade service. 
    The facade service is then used by the timed sender service.
    The timed sender's method StoreDataFromSources() is called to store the data from the sources.
    */
    private static async Task IntitializeUtilityLists()
    {
        _utilityService = new UtilityServiceImpl(new UtilityRepoImpl(), new Logger<UtilityServiceImpl>(new LoggerFactory()));
        _eboksService = new EboksServiceImpl(new ScrapperImpl(), new Logger<EboksServiceImpl>(new LoggerFactory()));
        _rtoDataService = new RtoDataServiceImpl(new RtoRepoImpl(), new UtilityRepoImpl(), new Logger<RtoDataServiceImpl>(new LoggerFactory()));

        _facade = new FacadeImpl(_utilityService, _eboksService, _rtoDataService);
        _timedSender = new DataPullerImpl(_facade, new Logger<DataPullerImpl>(new LoggerFactory()));

        await _timedSender.StoreDataFromSources();
    }

    //Asserts that the utilityData is not null
    [TestMethod]
    public async Task TestGettingUtilityData()
    {
        var utilityData = DataHolderSingleton.Instance.UtilityData;
        Assert.IsTrue(utilityData != null);
    }

    //Asserts that the latestRTOData is not null
    [TestMethod]
    public async Task TestGettingLatestRTOData()
    {
        var latestRTOData = DataHolderSingleton.Instance.LatestRTOData;
        Assert.IsTrue(latestRTOData != null);
    }

    //Asserts that the eboks is not null
    [TestMethod]
    public async Task TestGettingEboksData()
    {
        var eboksData = DataHolderSingleton.Instance.EboksData;
        Assert.IsTrue(eboksData != null);
    }

    //Asserts that the eboksData is a digit
    [TestMethod]
    public async Task TestEboksDataIsNumericString(){
        var eboksData = DataHolderSingleton.Instance.EboksData;
        Assert.IsTrue(eboksData.All(char.IsDigit));
    }

    //Asserts that the eboksData is not null
    [TestMethod]
    public async Task TestGettingUtilitiesWithZones()
    {
        var utilitiesWithZonesByDate = DataHolderSingleton.Instance.UtilitiesWithZonesByDate;
        Assert.IsTrue(utilitiesWithZonesByDate !=null);
    }





}