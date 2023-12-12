using Microsoft.Extensions.Logging;
namespace backend_test.UserPrefTest;

[TestClass]
public class UserPrefTest
{

    private readonly UserPrefImpl _userPrefRepository;
    private readonly UserPrefServiceImpl userPrefService;

    /*Initialising the dependencies for the userPrefService and the service itself*/
    public UserPrefTest()
    {
        ILogger<UserPrefImpl> logger = new Logger<UserPrefImpl>(new LoggerFactory());
        _userPrefRepository = new UserPrefImpl(logger);

        ILogger<UserPrefServiceImpl> logger2 = new Logger<UserPrefServiceImpl>(new LoggerFactory());
        userPrefService = new UserPrefServiceImpl(_userPrefRepository, logger2);

    }

    // Testing creation of user prefrences
    [TestMethod]
    public async Task TestCreateUserPref()
    {
        var userPref = new UserPref{
            UserId = "test",
            Large = new GridLayout(),
            Medium = new GridLayout(),
            Small = new GridLayout()
        };

        await userPrefService.AddUserPrefToFile(userPref);

        var createdUserPref = await userPrefService.GetUserPrefFromFile(userPref.UserId);

        Assert.IsTrue(createdUserPref.UserId == userPref.UserId);
        
    }   


    // Testing deletion of user prefrences
    [TestMethod]
    public async Task TestDeleteUserPref()
    {
        var userPref = new UserPref{
            UserId = "testDeletion",
            Large = new GridLayout(),
            Medium = new GridLayout(),
            Small = new GridLayout()
        };

        await userPrefService.AddUserPrefToFile(userPref);
        await userPrefService.DeleteUserPrefFromFile(userPref.UserId);

        var deletedUserPref = await userPrefService.GetUserPrefFromFile(userPref.UserId);

        Assert.IsTrue(deletedUserPref == null);
    }

    // Testing creating user preference for a user that already exists
    [TestMethod]
    public async Task TestCreatePrefForUserWithExistentPref(){
        
        string userId = "testDuplicate";

        var userPrefOriginal = new UserPref{
            UserId = userId,
            Large = new GridLayout{
                Tiles =    new List<Tile>(){
                    new Tile{
                        i = "test",
                        x = 0,
                        y = 0,
                        w = 0,
                        h = 0,
                    },
                
                },
            },
        };

        var userPrefDuplicate = new UserPref{
            UserId = userId,
            Large = new GridLayout{
                Tiles = new List<Tile>(),
            },
        };

        await userPrefService.AddUserPrefToFile(userPrefOriginal);
        await userPrefService.AddUserPrefToFile(userPrefDuplicate);

        var createdUserPref = await userPrefService.GetUserPrefFromFile(userId);

        Assert.IsTrue(!createdUserPref.Large.Tiles.Any());

    }

    // Testing creating a null user preference
    [TestMethod]
    public async Task TestCreateNullUserPref(){
        UserPref userPref = null;

        var createdUserPref = await userPrefService.AddUserPrefToFile(userPref);

        Assert.IsTrue(createdUserPref == 400);
    }
    
    //Testing creating user preference with userId = null
    [TestMethod]
    public async Task CreateUserPrefWithNullUserId(){
        var userPref = new UserPref{
            UserId = null,
            Large = new GridLayout(),
            Medium = new GridLayout(),
            Small = new GridLayout()
        };

        var createdUserPref = await userPrefService.AddUserPrefToFile(userPref);

        Assert.IsTrue(createdUserPref == 400);
    }

    
}