using UnityEngine;
using System.Collections;

public class DisasterManager : MonoBehaviour {
	public GameObject Tornado;
	public GameObject Terremoto;
	public GameObject Epidemia;
	public Transform Spawnpoint;
	public CityManager CM;
	// Use this for initialization
	void Start () {

	}

//	public void GetDisasterFromCall(string Type)
//	{
//		StartCoroutine (makeDisaster(Type));
//	}
//
//	IEnumerator makeDisaster(string Type)
//	{
//		switch (Type) 
//		{
//		case "_tornado":
//			Tornado.SetActive (true);
//			ParticleSystem tornado = Tornado.GetComponent<ParticleSystem> ();
//			tornado.Clear ();
//			tornado.Play ();
//			yield return new WaitForSeconds (5);
//			Tornado.SetActive (false);
//			CM._ciudadanosSinCasa += Random.Range (10, 50);
//			CM.HogaresT.text = CM._ciudadanosSinCasa.ToString ();
//				break;
//			case "_terremoto":
//				Terremoto.SetActive (true);
//				ParticleSystem terremoto = Terremoto.GetComponent<ParticleSystem> ();
//				terremoto.Clear ();
//				terremoto.Play ();
//				yield return new WaitForSeconds (5);
//				Terremoto.SetActive (false);
//			CM._ciudadanosSinCasa += Random.Range (10, 30);
//			CM.HogaresT.text = CM._ciudadanosSinCasa.ToString ();
//				break;
//			case "_epidemia":
//				Epidemia.SetActive (true);
//				ParticleSystem epidemia = Epidemia.GetComponent<ParticleSystem> ();
//				epidemia.Clear ();
//				epidemia.Play ();
//			CM._ciudadanosEnfermos += Random.Range (10, 20);
//			CM.EnfermosT.text = CM._ciudadanosEnfermos.ToString ();
//				yield return new WaitForSeconds (5);
//				Epidemia.SetActive (false);
//				break;
//		}
////		CM.SetNextDisaster ();
//	}
}

