using UnityEngine;
using System.Collections;

public class WaterManagementScript : MonoBehaviour {

	public Renderer waterRenderer;
	public float textureOffset = 0.0f;
	// Use this for initialization
	void Start () {
	
	}


	// Update is called once per frame
	void Update () {
		textureOffset = Mathf.Lerp (textureOffset, 1000.0f, Time.deltaTime / 10000.0f);
		waterRenderer.material.mainTextureOffset = new Vector2 (textureOffset, waterRenderer.material.mainTextureOffset.y);

	}
}
