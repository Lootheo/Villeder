using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoseScript : MonoBehaviour {
	public Text TimeText;
	DataLevel _data;

	void Start()
	{
		_data = FindObjectOfType<DataLevel> ();
		TimeText.text = _data.TotalTime;
	}

	public void Retry()
	{
		if(_data.LevelType == 1)
			SceneManager.LoadScene ("GameDefault");
		else if(_data.LevelType == 2)
			SceneManager.LoadScene ("GameDessert");
	}

	public void GoToMainMenu()
	{
		SceneManager.LoadScene ("MainMenu");
	}

	public void GoToTutorial()
	{
		SceneManager.LoadScene ("TutorialLevel");
	}
}
