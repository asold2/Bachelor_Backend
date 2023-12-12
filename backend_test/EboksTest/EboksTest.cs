using Microsoft.Extensions.Logging;

namespace backend_test.EboksTest;

[TestClass]
public class EboksTest{

    private static EboksServiceImpl _eboksServiceImpl;
    

    [ClassInitialize]
    public static void InitializeTestClass(TestContext context)
    {
        IntitializeUtilityLists().Wait(); // Wait for the async method to complete
    }
    /*Initialising eboks service with its dependencies*/
    private static async Task IntitializeUtilityLists()
    {
        _eboksServiceImpl = new EboksServiceImpl(new ScrapperImpl(), new Logger<EboksServiceImpl>(new LoggerFactory()));
    }

    public EboksTest(){}

    // Testing that number of eboks services down is not negative
    [TestMethod]
    public async Task TestGetEboksData(){
        var eboksData = await _eboksServiceImpl.GetNumberOfServicesDown();
        Assert.IsTrue(eboksData >= 0);
    }
}