using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseStartupManager : MonoBehaviour
{
    public FirebaseApp app { get; private set; }
    public static FirebaseStartupManager instance;

    public DatabaseReference databaseReference { get; private set; }

    bool _appInitialized = false;
    bool _dbInitialized = false;

    [Header("User")]
    public bool resetUser; //generate a new user at each launch
    [Tooltip("Leave empty to generate an ID")]
    public string customPlayerID = "";
    public bool cleanUsersDB;
    public bool cleanGiveawayDB;
    [Space]
    public string defaultUsername = "";
    public string defaultContact = "";
    public int defaultScore = 0;

    const string usersRef = "users";
    const string giveawayRef = "users_concours";
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitAsync();
        DontDestroyOnLoad(gameObject);
    }

    async Task InitAsync()
    {
        await InitApp();
        InitDatabase();

        if (cleanGiveawayDB)
        {
            await CleanDatabase(cleanUsersDB);
        }

        if (resetUser) SETTINGS.playerID = customPlayerID;

        string pId = SETTINGS.playerID;

        if (string.IsNullOrEmpty(pId))
        {
            string id = await WriteUser(defaultUsername,defaultContact, defaultScore);
            SETTINGS.playerID = id;
        }
        else
        {
            //Check if in database, the one with all the users
            await databaseReference.Child(usersRef).Child(pId)
                .GetValueAsync().ContinueWithOnMainThread(async task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("could not resolve database");
                    }else if (task.IsCompleted)
                    {
                        DataSnapshot dataSnapshot = task.Result;

                        if (dataSnapshot.Exists)
                        {
                            Debug.Log("user already in the database");
                        }
                        else
                        {
                            await WriteUser(defaultUsername, defaultContact, defaultScore, true, false, SETTINGS.playerID);
                        }
                    }
                });
        }
        await UpdateGiveawaySettingsAsync();
    }

    async Task UpdateGiveawaySettingsAsync()
    {
        await databaseReference.Child(giveawayRef).Child(SETTINGS.playerID)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("Giveaway check failed");
                }else if (task.IsCompleted)
                {
                    DataSnapshot dataSnapshot = task.Result;

                    SETTINGS.isGiveaway = dataSnapshot.Exists;
                }
            });
    }

    /// <summary>
    /// Clear all entries in the db
    /// </summary>
    /// <param name="fullClean">Also clean the base users</param>
    /// <returns></returns>
    async Task CleanDatabase(bool fullClean)
    {
        await databaseReference.Child(giveawayRef).RemoveValueAsync();
        if (fullClean) await databaseReference.Child(usersRef).RemoveValueAsync();
    }

    public DatabaseReference GetGiveawayDB()
    {
        return databaseReference.Child(giveawayRef);
    }

    public DatabaseReference GetUsersDB()
    {
        return databaseReference.Child(usersRef);
    }

    async Task InitApp()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = FirebaseApp.DefaultInstance;
                _appInitialized = true;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError(string.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    void InitDatabase()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    /// <summary>
    /// Write a user to the database
    /// </summary>
    /// <param name="username">Username of the player given on entering the giveaway. use default username for init</param>
    /// <param name="contact">Mail of phone of the player given on entering the giveaway. Leave empty for init</param>
    /// <param name="hidden">Hide in the leaderboard</param>
    /// <param name="score">Number of time the player finished the game</param>
    /// <param name="overwriteKey">key used to overwrite an already present user in the db</param>
    /// <returns></returns>
    public async Task<string> WriteUser(string username, string contact, int score, 
        bool hidden = true, bool giveaway = false, string overwriteKey = "", string title="")
    {
        string key;

        if (string.IsNullOrEmpty(overwriteKey))
        {
            key = databaseReference.Child(usersRef).Push().Key;
        }
        else
        {
            key = overwriteKey;
        }
        
        User user = new User(username, contact, System.DateTime.Now.ToShortDateString(), score, hidden, title);
        Dictionary<string, System.Object> entryValue = user.ToDictionary();

        Dictionary<string, System.Object> childUpdate = new Dictionary<string, System.Object>();
        childUpdate[usersRef + "/" + key] = entryValue;
        if (giveaway)
        {
            childUpdate[giveawayRef + "/" + key] = entryValue;
        }

        await databaseReference.UpdateChildrenAsync(childUpdate);

        return key;
    }
}
