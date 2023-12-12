using backend.Services.SignalR;
using Microsoft.Extensions.Logging;
namespace backend_test.AzureHubTest;

[TestClass]
public class AzureHubTest{

    private static AzureHubImpl _azureHub;
    
    [ClassInitialize]
    public static void InitializeTestClass(TestContext context)
    {
        IntitializeUtilityLists().Wait(); // Wait for the async method to complete
    }
    /*Initialising the dependency for the azure hub service, 
    the azure hub service itself */
    
    private static async Task IntitializeUtilityLists()
    {
        _azureHub = new AzureHubImpl(new Logger<AzureHubImpl>(new LoggerFactory()), null);
    }

    public AzureHubTest(){}

    // Testing that the hub has 0 connections when it's started
    [TestMethod]
    public async Task TestAzureHubHas0Connections(){
        Assert.IsTrue(_azureHub.GetNumberOfConnectedClients() == 0);
    }

}
