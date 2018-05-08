using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameEnum {

	[System.Serializable]
	public enum StateAnswer
	{
		correct = 0,
		incorrect,
		timeout
	}

	[System.Serializable]
	public enum Language
	{
		thai = 0,
		english
	}

	[System.Serializable]
    public enum CountdownType
    {
        number = 0,
        clock,
		guage
    }

	[System.Serializable]
	public enum ScoreType
	{
		normal = 0,
		kinect
	}
}
