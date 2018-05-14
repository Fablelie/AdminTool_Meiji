using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealtimeScoreBoard : MonoBehaviour {

	public GameObject prefabCell;
	public RectTransform content;
	public List<RealtimeScoreCell> cellList;

	[SerializeField] private RankingPanel rankingPanel;

	private float height;
	private float spacing;

	// private static RealtimeScoreBoard _instance;
	// public static RealtimeScoreBoard Instance 
	// {
	// 	get 
	// 	{
	// 		return _instance;
	// 	}
	// }

	void Awake()
	{
		// if(Instance == null)
		// {
		// 	_instance = this;
		// }

		height = 0;
        spacing = content.GetComponent<VerticalLayoutGroup>().spacing;
    }

	void OnEnable()
	{
		if(cellList != null)
			cellList.ForEach(cell => cell.OnSave());
	}

	public void NewCell(User user)
	{
		GameObject obj = Instantiate(prefabCell);
		obj.transform.SetParent(content);
		cellList.Add(obj.GetComponent<RealtimeScoreCell>().Init(user, this));
        RectTransform rt = obj.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(Screen.width, 350);
		rt.localScale = new Vector3(1, 1, 1);
		// print(Screen.width);
		// rt.width = Screen.width;
		height += rt.rect.height + spacing;

        content.sizeDelta = new Vector2(content.sizeDelta.x, height);
	}
	
	public void UpdateCell(User user)
	{
		cellList.ForEach(cell => {
			if(cell.deviceID == user.deviceID)
			{
				cell.UpdateCell(user);
				return;
			}
		});
	}

	public void GotoRankingPanel()
	{
		cellList.ForEach(cell =>
		{
			if(!cell.isSave) cell.OnSave();
		});
		
		cellList.Sort();
		for (int i = 0; i < rankingPanel.cellList.Count; i++)
		{
			rankingPanel.cellList[i].SetupRankingCell(
				mascot: cellList[i].mainSprite,
				name: cellList[i].teamName.text,
				score: cellList[i].score.ToString()
			);
		}
		
		rankingPanel.gameObject.SetActive(true);
		gameObject.SetActive(false);

	}

}
