using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class AndroidConnection : MonoBehaviour {
	public GameObject LoadingUI;
	public GameObject BaseUI;
	public GameObject PauseUI;
	public CityManager CM;
    AndroidJavaObject instance = null;
    AndroidJavaObject activityInstance = null;
    AndroidJavaObject activityContext = null;
    // Use this for initialization
    void Start () {
		#if UNITY_ANDROID && !UNITY_EDITOR
        using(AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
            activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
        }
        //
		using(AndroidJavaClass pluginClass = new AndroidJavaClass("com.test.fenrir.hackaton.Manager")) {
            if(pluginClass != null) {
                instance = pluginClass.CallStatic<AndroidJavaObject>("instance");
                instance.Call("setContext", activityContext);
            }
        }
		#endif
    }

    public void MainMenuScene()
    {
		PackData ();
		StartCoroutine (LoadSceneAfter());
    }

	void OnApplicationQuit()
	{
		PackData ();
	}

	void PackData()
	{
		StatsData _data = new StatsData ();
		if (CM != null) {
			_data.dinero = CM._presupuesto;
			_data.comida = CM._comida;
			_data.aldeanosTotal = CM._poblacion;
			_data.aldeanosEnfermos = CM._ciudadanosEnfermos;
			_data.aldeanosHambrientos = CM._ciudadanosSinComida;
			BuildingsCreated[] _construcciones = CM.creations.ToArray();
			_data._totalBuildings = _construcciones;
			StoreData.Save (_data);
		}
	}

	IEnumerator LoadSceneAfter()
	{
		BaseUI.SetActive (false);
		PauseUI.SetActive (false);
		LoadingUI.SetActive (true);
		Animation anim = LoadingUI.GetComponent<Animation> ();
		AnimationClip clip = anim.GetClip ("LoadIn");
		anim.Play (clip.name);
		yield return new WaitForSeconds (clip.length);
		SceneManager.LoadScene("MainMenu");
	}

    public void CallModal()
    {
		#if UNITY_ANDROID && !UNITY_EDITOR
        activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
		instance.Call("ProgressDialog");
        }));
		#endif
    }

	public void GetPath()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
			instance.Call("GetDataPath", Application.persistentDataPath);
		}));
		#endif
	}

    public void ShareBtn()
    {
		#if UNITY_ANDROID && !UNITY_EDITOR
        activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
            instance.Call("shareText", "Villeder es Genial!!", "Diviertete y ayuda a combatir la pobreza al mismo tiempo!");
        }));
		#endif
    }

    public void OpenLink()
    {
		#if UNITY_ANDROID && !UNITY_EDITOR
        activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
            instance.Call("ActionViewIntent");
        }));
		#endif
    }

//    public void CancelNotification()
//    {
//		#if UNITY_ANDROID && !UNITY_EDITOR
//        activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
//            instance.Call("CancelNotification");
//        }));
//		#endif
//    }

	public void CallDialog()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
		instance.Call("ProgressDialog");
		}));
		#endif
	}

//    public void CancelAllNotification()
//    {
//		#if UNITY_ANDROID && !UNITY_EDITOR
//        activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
//		instance.Call("CancelAllNotifications");
//        }));
//		#endif
//    }
//
//    public void CallNotificacion()
//    {
//		#if UNITY_ANDROID && !UNITY_EDITOR
//        activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
//            instance.Call("LaunchBroadcast", 5);
//        }));
//		#endif
//    }

//	public void CallDisaster()
//	{
//		#if UNITY_ANDROID && !UNITY_EDITOR
//		int RandType = Random.Range (1, 4);
//		int RandTime = Random.Range (10, 20);
//		activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() => {
//			instance.Call("DangerBroadcast", RandTime, RandType);
//		}));
//		#endif
//	}
}
