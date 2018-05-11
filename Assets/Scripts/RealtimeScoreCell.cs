using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealtimeScoreCell : MonoBehaviour, IComparable<RealtimeScoreCell> {

	public InputField teamName;
    [SerializeField] private InputField totalScore;
    [SerializeField] private Button saveBtn;
    [SerializeField] private Text btnText;
	[SerializeField] private List<Text> stationScoreList;

    public Text specialStation;

	public string deviceID;

	public bool isSave = false;
    public int score;

    public Sprite mainSprite;

    public Sprite[] mascots;

	public RealtimeScoreCell Init(User user, RealtimeScoreBoard realtimeScoreBoard)
	{
        // saveBtn.onClick.AddListener(() => realtimeScoreBoard.GotoRankingPanel());
		UpdateCell(user);
		return this;
	}

	public void UpdateCell(User user)
	{
        if(!isSave)
        {
            deviceID = user.deviceID;
            teamName.text = user.groupName;
            totalScore.text = user.score.ToString();
            mainSprite = GetMascotSprite(user.mascotId);
            int index = 0;
            user.station.ForEach(score =>
            {
                stationScoreList[index].text = score.ToString();
                index++;
            });

            specialStation.text = user.specialScore.ToString();
        }
	}

    public Sprite GetMascotSprite(int mascotId)
    {
        try
        {
            return mascots[mascotId - 1];    
        }
        catch (System.Exception)
        {
            return mascots[0];
        }
        
    }

    public void OnSave()
    {
        isSave = !isSave;
        score = int.Parse(totalScore.text);

        teamName.interactable = totalScore.interactable = !isSave;
        
        btnText.text = (isSave) ? "EDIT" : "SAVE";
    }

	private string GetTeamName(string name)
	{
        switch (name)
        {
            case "DD":
                mainSprite = mascots[0];
                return "ทีม : พี่ดีดี";
            case "BOW":
                mainSprite = mascots[1];
                return "ทีม : พี่โบว์";
            case "POLY":
                mainSprite = mascots[2];
                return "ทีม : พี่พลอย";
            case "EARTH":
                mainSprite = mascots[3];
                return "ทีม : พี่เอิร์ธ";
            default:
                mainSprite = mascots[3];
                return name;
        }
	}

    public void UpdateGroupName()
    {
        Server.Instance.UpdateGroupName(teamName.text, deviceID);
    }

    public void UpdateTotalScore()
    {
        Server.Instance.UpdateScore(int.Parse(totalScore.text), deviceID);
    }
    
	public void OnClickSaveBtn()
	{
		teamName.interactable = !teamName.interactable;
		totalScore.interactable = !totalScore.interactable;

    }

    public int CompareTo(RealtimeScoreCell other)
    {
        return (score > other.score) ? -1 : (score == other.score) ? 0 : 1;
    }
}
