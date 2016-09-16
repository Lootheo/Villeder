using UnityEngine;
using System.Collections;

public class DataLevel : MonoBehaviour {
	public string TotalTime;
	public int LevelType;

	void Start()
	{
		DontDestroyOnLoad (this.gameObject);
	}
}
