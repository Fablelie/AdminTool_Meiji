using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingPanel : MonoBehaviour {
	public Button screenBtn;

	public List<RankingCell> cellList;

	void OnEnable()
	{
        screenBtn.gameObject.SetActive(true);
		cellList.ForEach(list => list.gameObject.SetActive(false));
        
		screenBtn.onClick.RemoveAllListeners();
		screenBtn.onClick.AddListener(() => {
			cellList[3].gameObject.SetActive(true);
			screenBtn.onClick.RemoveAllListeners();

            screenBtn.onClick.AddListener(() => {
                cellList[2].gameObject.SetActive(true);
                screenBtn.onClick.RemoveAllListeners();

                screenBtn.onClick.AddListener(() => {
                    cellList[1].gameObject.SetActive(true);
                    cellList[0].gameObject.SetActive(true);
                    screenBtn.onClick.RemoveAllListeners();
                    screenBtn.gameObject.SetActive(false);
				});
			});
		});
	}

	public void ResetGame()
	{
		
	}


}
