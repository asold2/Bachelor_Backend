namespace backend_test.UtilityTest;

using Microsoft.Extensions.Logging;
using Models.UtilityCalculationRow;

[TestClass]
public class UtilityTests
{

    private static UtilityServiceImpl _utilityService;
    private static UtilityRepoImpl _utilityRepo;
    private static  List<UtilityZonesDateRow> utilityZonesDateRows;
    private static  List<UtilityCalculationRow> utilityCalculationRows;

    public UtilityTests()
    {

    }

    [ClassInitialize]
    public static void InitializeTestClass(TestContext context)
    {
        IntitializeUtilityLists().Wait(); // Wait for the async method to complete
    }
    /*Initialising the dependency for the utility service, 
    the utility service itself, 
    as well as the two lists that the utility service produces */
    private static async Task IntitializeUtilityLists()
    {
        _utilityRepo = new UtilityRepoImpl();
        _utilityService = new UtilityServiceImpl(_utilityRepo, new Logger<UtilityServiceImpl>(new LoggerFactory()));
        utilityCalculationRows = await _utilityService.GetAzureLogs();
        utilityZonesDateRows = await _utilityService.GetUtilitesWithZonesByDate();
    }

    // Testing that the utilityCalculationRows list has elements in it
    [TestMethod]
    public async Task TestGettingCalculationRows()
    {
        Assert.IsTrue(utilityCalculationRows.Count > 0);
    }

    // Testing that the first element of the utilityCalculationRows does not have calculations in the last 24 hours
    [TestMethod]
    public async Task TestFirstElementOfCalculationRowsStartsWith1(){
        Assert.IsFalse(utilityCalculationRows[0].CalculationLast24Hours);
    }

    // Testing that the utilityZonesDateRows list has elements in it

    [TestMethod]
    public async Task TestGettingZonesDateRows()
    {
        Assert.IsTrue(utilityZonesDateRows.Count > 0);
    }

    // Testing that the first element of the utilityZonesDateRows list has a rowId of 1
    [TestMethod]
    public async Task TestFirstElementOfZonesDateRowsStartsWith1(){
        Assert.IsTrue(utilityZonesDateRows[0].RowId == 1);
    }
}