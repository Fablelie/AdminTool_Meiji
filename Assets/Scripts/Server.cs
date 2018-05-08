using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using UnityEngine.UI;

public class Server : MonoBehaviour {

    public static Server instance;
    public static Server Instance
    { 
        get 
        {
            return instance;
        } 
        set 
        {
            instance = value;
        }
    }

    [SerializeField] private InputField usernameInput;
    [SerializeField] private InputField passwordInput;
    [SerializeField] private GameObject loginPanel;

    [SerializeField] private string deviceID;

    [SerializeField] private RealtimeScoreBoard realtimeScoreBoard;

    private DatabaseReference reference;

    [SerializeField] private List<User> listUserData;
    
    private string queryDeviceID;
    private string groupName;
    private int score;
    // private List<int> station = new List<int>(new int[6]);
    private int specialScore;
    private int kinectScore;
    private const string normal = "score";
    private const string kinect = "kinectScore";

    void Awake() {
        if(instance == null) instance = this;
        // deviceID = SystemInfo.deviceUniqueIdentifier;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://meiji-questions-game.firebaseio.com/");
        listUserData = new List<User>();
        CheckUserAdmin();
        // reference.ValueChanged += HandleValueChanged;
        // print("deviceUniqueIdentifier : " + deviceID);
        // WriteNewUser("eiei", 100, 200);
        // reference.Child(deviceID).Child("score").SetValueAsync(700);
        // GetUpdateScoreBoard();
        
    }

    public void CheckUserAdmin()
    {
        reference = FirebaseDatabase.DefaultInstance.GetReference("user-admin");
        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                print("Error");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();
                bool isAdmin = true;
                foreach (DataSnapshot s in snapshot.Children)
                {
                    print(s.Key + " : " + s.Value.ToString());
                    switch (s.Key)
                    {
                        case "username":

                            if(s.Value.ToString() != usernameInput.text)
                            {
                                print(s.Value.ToString() + " != " + usernameInput.text);
                                isAdmin = false;
                            }
                            break;
                        case "password":
                            if (s.Value.ToString() != passwordInput.text)
                            {
                                print(s.Value.ToString() + " != " + passwordInput.text);
                                isAdmin = false;
                            }
                            break;
                    }    
                }
                OpenEditScore(isAdmin);
            }
        });
    }

    public void OpenEditScore(bool isAdmin)
    {
        if(isAdmin)
        {
            loginPanel.gameObject.SetActive(false);
            realtimeScoreBoard.gameObject.SetActive(true);
            reference = FirebaseDatabase.DefaultInstance.GetReference("user-data");
            reference.ValueChanged += HandleValueChanged;
        }
        else 
        {

        }
    }

    public string GetDeviceId()
    {
        return deviceID;
    }

    private void InsertUpdateListUserData(User currentUserData)
    {
        User user = listUserData.Find(u => u.deviceID == currentUserData.deviceID);
        if(user != null)
        {
            print(currentUserData.groupName);
            currentUserData.station.ForEach(score => print(score));
            user.UpdateUserData(currentUserData);
            realtimeScoreBoard.UpdateCell(currentUserData);
            return;
        }

        print(currentUserData.groupName);
        currentUserData.station.ForEach(score => print(score));
        listUserData.Add(currentUserData);
        realtimeScoreBoard.NewCell(currentUserData);
    }

    public List<User> GetListUser()
    {
        listUserData.Sort();
        return listUserData;
    }

    private void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot

        DataSnapshot snapshot = args.Snapshot;

        string json = snapshot.GetRawJsonValue();
        // print(json);
        // listUserData = new List<User>();
        foreach (DataSnapshot s in snapshot.Children)
        {
            // print(s.Key);
            queryDeviceID = s.Key;
            int mascotId = 0;
            List<int> stationScore = new List<int>(new int[6]);
            foreach (DataSnapshot item in s.Children)
            {
                switch (item.Key)
                {
                    case "groupName":
                        groupName = item.Value.ToString();
                        break;
                    case "score":
                        score = int.Parse(item.Value.ToString());
                        break;
                    case "kinectScore":
                        kinectScore = int.Parse(item.Value.ToString());
                        break;
                    case "station":
                        foreach (var stationIndex in item.Children)
                        {
                            // Debug.LogError(stationIndex.Key);
                            int index = int.Parse(stationIndex.Key);
                            int value = int.Parse(stationIndex.Value.ToString());
                            stationScore[index] = value;
                        }
                        break;
                    case "specialScore":
                        specialScore = int.Parse(item.Value.ToString());
                        break;
                    case "mascotId":
                        mascotId = int.Parse(item.Value.ToString());
                        break;
                }
            }
            InsertUpdateListUserData(new User(queryDeviceID, groupName, score, kinectScore, specialScore, stationScore, mascotId));
        }
    }

    public void WriteNewUser (string groupName, int score = 0, int kinectScore = 0)
    {
        User user = new User(deviceID, groupName, score, kinectScore);
        string json = JsonUtility.ToJson(user);
        print(json);
        reference.Child(deviceID).SetRawJsonValueAsync(json);
    }

    public void UpdateScore(int score, string userDeviceID)
    {
        reference.Child(userDeviceID).Child(normal).SetValueAsync(score);
    }

    public void UpdateStationScore(int stationIndex, int score, string userDeviceID)
    {
        reference.Child(userDeviceID).Child("station").Child(stationIndex.ToString()).SetValueAsync(score);
    }

    public void UpdateGroupName(string name, string userDeviceID)
    {
        reference.Child(userDeviceID).Child("groupName").SetValueAsync(name);
    }

    public string GetGroupName()
    {
        try
        {
            return listUserData.Find(user => user.deviceID == deviceID).groupName;    
        }
        catch (System.Exception)
        {
            WriteNewUser("NewUser");
            return "New User Restart game for genarate user.";
        }
        
    }
	
}
