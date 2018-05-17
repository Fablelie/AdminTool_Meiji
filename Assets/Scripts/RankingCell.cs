using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingCell : MonoBehaviour {

	public Image mascot;
	public Text teamName;
	public Text score;

	public void SetupRankingCell(Sprite mascot, string name, string score)
	{
		this.mascot.sprite = mascot;
		this.teamName.text = name;
		this.score.text = score;
	}
}
