using UnityEngine;
using System.Collections;

public class GranjaScript : MonoBehaviour {


	public int currentResources = 0;
	public int tickResources = 5;
	public int maxAmount = 25;
	public Material NotSelectedMaterial;
	public Material ResourcesMaterial;
	public Material SelectedMaterial;


	// Use this for initialization
	void Start () {
		StartCoroutine (TickGenerator ());
		gameObject.GetComponent<Renderer> ().material = NotSelectedMaterial;
	}

	public void GenerateTick(){
		if (currentResources <= 0) {
			currentResources = 0;
			gameObject.GetComponent<Renderer> ().material = ResourcesMaterial;
		}

		if (maxAmount > currentResources) {
			currentResources += tickResources;
		}

		if (currentResources > maxAmount) {
			currentResources = maxAmount;
		}
	}

	public int ConsumeResource(){
		currentResources -= tickResources;
		if (currentResources <= 0) {
			currentResources = 0;
			gameObject.GetComponent<Renderer> ().material = NotSelectedMaterial;
		}
		return tickResources;
	}


	//Function to be replaced after
	public IEnumerator TickGenerator(){
		while (true) {
			yield return new WaitForSeconds (3.0f);
			GenerateTick ();
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}

