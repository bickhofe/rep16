using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour {

	public int speed = 10;

	// Use this for initialization
	void Start () {
		iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("birdpath"), "orientToPath", true, "lookTime", 0.5f, "time", speed, "easetype", iTween.EaseType.linear, "loopType", "loop"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
