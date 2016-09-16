using UnityEngine;
using System.Collections;

public class SoundControl : MonoBehaviour {
	AudioSource soundControl;
	public AudioClip myAudioClip;
	// Use this for initialization
	void Start () {
		
	}
	public void PlaySound(){
		soundControl =  GameObject.FindGameObjectWithTag ("SoundControl").GetComponent<AudioSource>();
		soundControl.clip = myAudioClip;
		if (!soundControl.isPlaying) {
			soundControl.Play ();
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
