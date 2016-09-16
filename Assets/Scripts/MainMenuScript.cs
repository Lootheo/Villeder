using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainMenuScript : MonoBehaviour {
	public DataStorage ds;
    public GameObject MainMenu;
    public GameObject StoreMenu;
    public GameObject Loading;
	public GameObject LogrosMenu;
	public GameObject LevelSelection;
    public Animation _menu, _store, _load, _trophys, levels;
	public GameObject ContinueButton;
	// Use this for initialization

	void Awake()
	{
		ds = GameObject.FindObjectOfType<DataStorage> ();
		if (File.Exists (Application.persistentDataPath + "/StatsData.txt")) {
			ContinueButton.SetActive (true);
		} else {
			ContinueButton.SetActive (false);
		}
	}

    void Start () {
        StoreMenu.SetActive(false);
		Loading.SetActive (false);
		LogrosMenu.SetActive (false);
	}

    public void QuitGame()
    {
        Application.Quit();
    }

	public void ShowLevels()
	{
		StartCoroutine(playAnim("InLevels"));
		LevelSelection.SetActive(true);
	}

	public void HideLevels()
	{
		StartCoroutine(playAnim("OutLevels"));
		MainMenu.SetActive(true);
	}

    public void ShowStore()
    {
        StartCoroutine(playAnim("InStore"));
        StoreMenu.SetActive(true);
    }

	public void ShowAchivements()
	{
		StartCoroutine(playAnim("InTrophy"));
		LogrosMenu.SetActive(true);
	}

	public void HideAchivements()
	{
		StartCoroutine(playAnim("OutTrophy"));
		MainMenu.SetActive(true);
	}

    public void ShowMain()
    {
        StartCoroutine(playAnim("InMenu"));
        MainMenu.SetActive(true);
    }

    public void ShowLoading()
    {
        StoreMenu.SetActive(false);
        StartCoroutine(playAnim("InLoad"));
        Loading.SetActive(true);
    }

	public void StartNewGame()
	{
		ds._dataStored = null;
		StoreData.Delete ();
		StoreMenu.SetActive(false);
		StartCoroutine(playAnim("InLoad"));
		Loading.SetActive(true);
	}

	public void ContinueGame()
	{
		ds._dataStored = StoreData.Load ();
		MainMenu.SetActive(false);
		StartCoroutine(playAnim("InLoad1"));
		Loading.SetActive(true);
	}

	public void LoadSkinLevel(int option)
	{
		if (option == 1) {
			StartCoroutine(playAnim("InLoad1"));
		}
		else if(option == 2) {
			StartCoroutine(playAnim("InLoad2"));
		}
	}

    IEnumerator playAnim(string AnimName)
    {
		switch (AnimName) {
		case "InStore":
			_menu.Play("OutMain");
			_store.Play("StoreIn");
			while (_menu.IsPlaying("OutMain"))
			{
				yield return null;
			}
			MainMenu.SetActive(false);
			break;
		case "InMenu":
			_menu.Play("InMain");
			_store.Play("StoreOut");
			while (_menu.IsPlaying("InMain"))
			{
				yield return null;
			}
			StoreMenu.SetActive(false);
			break;
		case "InLoad1":
			Loading.SetActive (true);
			levels.Play("LevelsOut");
			_load.Play("LoadIn");
			while (_load.IsPlaying("LoadIn"))
			{
				yield return null;
			}
			LevelSelection.SetActive(false);
			yield return new WaitForSeconds(2);
			SceneManager.LoadScene("TutorialLevel");
			break;
		case "InLoad2":
			Loading.SetActive (true);
			levels.Play("LevelsOut");
			_load.Play("LoadIn");
			while (_load.IsPlaying("LoadIn"))
			{
				yield return null;
			}
			LevelSelection.SetActive(false);
			yield return new WaitForSeconds(2);
			SceneManager.LoadScene("GameDessert");
			break;
		case "InTrophy":
			_menu.Play("OutMain");
			_trophys.Play("StoreIn");
			while (_trophys.IsPlaying("StoreIn"))
			{
				yield return null;
			}
			MainMenu.SetActive(false);
			break;
		case "OutTrophy":
			_menu.Play("InMain");
			_trophys.Play("StoreOut");
			while (_menu.IsPlaying("InMain"))
			{
				yield return null;
			}
			LogrosMenu.SetActive(false);
			break;
		case "InLevels":
			_menu.Play("OutMain");
			levels.Play("LevelsIn");
			while (_trophys.IsPlaying("LevelsIn"))
			{
				yield return null;
			}
			MainMenu.SetActive(false);
			break;
		case "OutLevels":
			_menu.Play("InMain");
			levels.Play("LevelsOut");
			while (_menu.IsPlaying("InMain"))
			{
				yield return null;
			}
			LevelSelection.SetActive(false);
			break;
		}
    }
}
