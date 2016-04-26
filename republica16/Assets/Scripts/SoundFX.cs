using UnityEngine;
using System.Collections;

public class SoundFX : MonoBehaviour {

	public AudioClip gameStart;
	public AudioClip gameOver;
	public AudioClip pick;
	public AudioClip drop;
	public AudioClip fallwater;
	public AudioClip senditem;
	public AudioClip tick;
	public AudioClip drown;
	public AudioClip tuberide;

	AudioSource source;
	
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
		//PlayAudio(gameOver);
	}
	
	// Update is called once per frame
	public void PlayAudio (AudioClip snd) {
		source.PlayOneShot(snd,1);
	}


}
