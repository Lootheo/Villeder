using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FadeInScript : MonoBehaviour {

	public float transparency = 0.0f;
	public Image backGround;
	public List<Image> menuImages;
	public List<Text> menuTexts;
	public bool startTransition=false;

	// Use this for initialization

	float timeLerped = 0; 



	void Start () {
		transparency = 0;
		foreach (Image menuImage in menuImages) {
			menuImage.color = new Color (backGround.color.r, backGround.color.g, backGround.color.b, transparency);
		}
		foreach (Text menuText in menuTexts) {
			menuText.color = new Color (menuText.color.r, menuText.color.g, menuText.color.b, transparency);
		}
		StartCoroutine (StartTransition ());
	}

	IEnumerator StartTransition(){
		yield return new WaitForSeconds (2.5f);
		startTransition = true;
	}
	// Update is called once per frame
	void Update () {
		float timeToLerp = 2.3f; //lerp for two seconds.

		if (startTransition) {

			timeLerped += Time.deltaTime;
			transparency = Mathf.Lerp(0.0f, 1.0f, timeLerped / timeToLerp);

			foreach (Image menuImage in menuImages) {
				menuImage.color = new Color (backGround.color.r, backGround.color.g, backGround.color.b, transparency);
			}
			foreach (Text menuText in menuTexts) {
				menuText.color = new Color (menuText.color.r, menuText.color.g, menuText.color.b, transparency);
			}
		}

	}
}
