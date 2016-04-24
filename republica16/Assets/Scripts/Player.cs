﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Main MainScript;

    public int playerID;
	public Vector3 playerPos;
    public float playerAngle;
    public Vector3 playerHead;
    public Color _color;
    public int curIsland;
	public bool updatePos = false;
	Vector3 camRotation;

	Collider col;

    // Use this for initialization
    void Start () {
        MainScript = GameObject.Find("Main").GetComponent<Main>();
		col = GetComponent<Collider>();

		if (playerID != MainScript.CharacterPlayerID) {
			//transform.Find("Character").GetComponent<Renderer>().material.color = _color;
		} else {
			//eigenen character unsichtbar machen
			//transform.Find("Character").gameObject.SetActive(false);
		}

        curIsland = playerID;
    }
	
	void Update() {
		if (MainScript.CharacterPlayerID == playerID) {
			//position von camera holen
			transform.position = MainScript.CamContainer.transform.position;
			camRotation = MainScript.MainCam.transform.eulerAngles;
			transform.eulerAngles = new Vector3(0, camRotation.y, 0); 

			//player pos parameter setzen
			playerPos = MainScript.CamContainer.transform.position;
			playerAngle = camRotation.y;
		} else {
			if (updatePos) {
				transform.position = playerPos;
				transform.eulerAngles = new Vector3(0, playerAngle, 0); 
				updatePos = false;
			}
		}
    }
}
