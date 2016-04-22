using UnityEngine;
using System.Collections;

public class Simulator : MonoBehaviour {

	public Main MainScript;
	public ServerComm ServerScript;

	float time = 0;
	public float updateTime = 1;

	// Use this for initialization
	void Start () {
		MainScript = GetComponent<Main>();
		ServerScript = GetComponent<ServerComm>();
	}
	
	// Update is called once per frame
	void Update () {

		//timer
		if (time < updateTime) time += Time.deltaTime;
		else {
			for (int i = 0; i < 4; i++) {
				if (i != MainScript.HumanPlayerID) {
					var rnd = Random.Range (-.1f, .1f);
					Vector3 testPos = MainScript.startPoint [i] + new Vector3 (rnd, 0, rnd);
					ServerScript.SendPlayerPos (i, testPos, Random.Range (0, 360));
				} else {
					ServerScript.SendPlayerPos (MainScript.HumanPlayerID, MainScript.Players[MainScript.HumanPlayerID].playerPos, MainScript.Players[MainScript.HumanPlayerID].playerAngle);
				}
			}

			time = 0;
		}
		
	}
}
