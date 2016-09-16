using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CameraRotateSmoothScript : MonoBehaviour {


	public Transform CamObject;
	public List<GameObject> currentTimers;
	public Quaternion camNextPosition;
	public CityManager manager;
	public Quaternion startingRotation;

	float timeLerped = 0.0f;
	// Use this for initialization
	void Start () {
		currentTimers = manager.currentTimers;
		startingRotation = transform.rotation;
		camNextPosition = transform.rotation;
		//CamObject = Camera.main.gameObject.transform.transform;
	}

	public void GirarCamara(bool _left)
	{
		if (_left) {
			timeLerped = 0.0f;
			camNextPosition = Quaternion.Euler (CamObject.eulerAngles.x, CamObject.eulerAngles.y + 90.0f, CamObject.eulerAngles.z);
			startingRotation = CamObject.rotation;
		} else {
			timeLerped = 0.0f;
			camNextPosition = Quaternion.Euler (CamObject.eulerAngles.x, CamObject.eulerAngles.y - 90.0f, CamObject.eulerAngles.z);
			startingRotation = CamObject.rotation;
			//StartCoroutine (TeOdioFenrirXOXO());
		}
	}

	// Update is called once per frame
	void Update () {
		StopAllCoroutines ();
		float timeToLerp = 1.0f; //lerp for two seconds.
		Vector3 startingPosition;
		if (CamObject.eulerAngles.y != camNextPosition.eulerAngles.y) {
			timeLerped += Time.deltaTime;
			CamObject.rotation = Quaternion.Lerp (startingRotation, camNextPosition, timeLerped / timeToLerp);
			if (currentTimers.Count > 0) {	
			for (int i = 0; i < currentTimers.Count; i++) {
				currentTimers [i].transform.LookAt (Camera.main.transform.position);
			}
		}
		}
	}

//	IEnumerator TeOdioFenrirXOXO () 
//	{
//		float timeToLerp = 1.0f; //lerp for two seconds.
//		Vector3 startingPosition;
//		while (CamObject.eulerAngles.y != camNextPosition.eulerAngles.y) {
//			timeLerped += Time.deltaTime;
//			CamObject.rotation = Quaternion.Lerp (startingRotation, camNextPosition, timeLerped / timeToLerp);
//			if (currentTimers.Count > 0) {	
//				for (int i = 0; i < currentTimers.Count; i++) {
//					currentTimers [i].transform.LookAt (Camera.main.transform.position);
//				}
//			}
//		}
//		yield return null;
//	}
}
