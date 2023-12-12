using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace backend_test.RTOTest;

[TestClass]
public class RtoTest{

    private static RtoDataServiceImpl _rtoDataServiceImpl;
    private static IUtilityService _utilityService;
    private static IRtoDataService _rtoDataService;
    private static IList<UtilityLatestCRMData> UtilitiesAndCalculationStatus;
    private static IList<UtilityLatestCRMData> UtilitiesWithNames;
    private static Dictionary<string, DateTimeOffset> CalculationsGroupedByCustomerId;
    private static Dictionary<string, DateTimeOffset> PulledDataGroupedByCustomerId;
    private readonly string pattern = @"\b[0-9a-f]{8}-([0-9a-f]{4}-){3}[0-9a-f]{12}\b";
    

    [ClassInitialize]
    public static void InitializeTestClass(TestContext context)
    {
        IntitializeUtilityLists().Wait(); // Wait for the async method to complete
    }
    /*Initialising the dependency for the rto serive, 
    the rto service itself, 
    as well as the two lists that the utility service produces */
    private static async Task IntitializeUtilityLists()
    {
         _utilityService = new UtilityServiceImpl(new UtilityRepoImpl(), new Logger<UtilityServiceImpl>(new LoggerFactory()));
        _rtoDataService = new RtoDataServiceImpl(new RtoRepoImpl(), new UtilityRepoImpl(), new Logger<RtoDataServiceImpl>(new LoggerFactory()));
        _rtoDataServiceImpl = new RtoDataServiceImpl(new RtoRepoImpl(), new UtilityRepoImpl(), new Logger<RtoDataServiceImpl>(new LoggerFactory()));
        UtilitiesAndCalculationStatus = await  _rtoDataServiceImpl.GetUtilityLatestRTOData();
        CalculationsGroupedByCustomerId = await _rtoDataServiceImpl.GetCalculationsGroup();
        PulledDataGroupedByCustomerId = await _rtoDataServiceImpl.GetPulledDataGroup();
    }
    
    public RtoTest(){}




    [TestMethod]
    public async Task TestUtilitiesAndCalculationStatusNotEmpty(){
        Assert.IsTrue(UtilitiesAndCalculationStatus.Count > 0);
    }

    [TestMethod]
    public async Task TestUtilitiesAndCalculationStatusStartsWith1(){
        Assert.IsTrue(UtilitiesAndCalculationStatus[0].RowId == 1);
    }

    [TestMethod]
    public async Task TestUtiltiesAndCalculationStatusHasAssignedNames(){
        Assert.IsTrue(UtilitiesAndCalculationStatus[0].UtilityName != "");
    }

    [TestMethod]
    public async Task TestUtiltiesAndCalculationStatusHasCorrectGuidFormat(){
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        Assert.IsTrue(regex.IsMatch(UtilitiesAndCalculationStatus[0].CustomerId));
    }

    [TestMethod]
    public async Task CheckThatAllElementsOfCaclualtionsDictHaveDifferentGuids(){
        var dict = new Dictionary<string, DateTimeOffset>();
        foreach (var key in CalculationsGroupedByCustomerId.Keys)
        {
            Assert.IsFalse(dict.ContainsKey(key));
            dict.Add(key, CalculationsGroupedByCustomerId[key]);
        }
    }

    [TestMethod]
    public async Task CheckThatAllElementsOfPulledDataDictHaveDifferentGuids(){
        var dict = new Dictionary<string, DateTimeOffset>();
        foreach (var key in PulledDataGroupedByCustomerId.Keys)
        {
            Assert.IsFalse(dict.ContainsKey(key));
            dict.Add(key, PulledDataGroupedByCustomerId[key]);
        }
    }


}