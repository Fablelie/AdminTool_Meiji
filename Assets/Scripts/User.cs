using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class User : IComparable<User>
{
    public string deviceID;
    public string groupName;
    public int score;
    public int kinectScore;
    public List<int> station;
    public int specialScore;
    public int mascotId;

    public User(string groupName, int score, int kinectScore) 
    { 
        this.groupName = groupName;
        this.score = score;
        this.kinectScore = kinectScore;
    }

    public User (string deviceID, string groupName, int score, int kinectScore)
    {
        this.deviceID = deviceID;
        this.groupName = groupName;
        this.score = score;
        this.kinectScore = kinectScore;
        this.specialScore = 0;
        this.station = new List<int>(new int[6]);
        UnityEngine.Debug.LogError("Station count : " + this.station.Count);
    }

    public User(string deviceID, string groupName, int score, int kinectScore, int specialScore, List<int> station, int mascotId)
    {
        this.deviceID = deviceID;
        this.groupName = groupName;
        this.score = score;
        this.kinectScore = kinectScore;
        this.specialScore = specialScore;
        this.station = station;
        this.mascotId = mascotId;
    }

    public void UpdateUserData(User user)
    {
        this.groupName = user.groupName;
        this.score = user.score;
        this.kinectScore = user.kinectScore;
        this.specialScore = user.specialScore;
        this.station = user.station;
        this.mascotId = user.mascotId;
    }

    public int CompareTo(User other)
    {
        return (score > other.score)? -1: (score == other.score)? 0:1;
    }
}
