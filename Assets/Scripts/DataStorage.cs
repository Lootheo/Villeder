using UnityEngine;
using System.Collections;

public class DataStorage : MonoBehaviour {
	public StatsData _dataStored;
	public bool _usingEvents;

	void Awake()
	{
		DontDestroyOnLoad (this);
	}

	public void ChangeEvents()
	{
		_usingEvents = !_usingEvents;
	}
}
