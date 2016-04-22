﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

    //debug
    public TextMesh debugTextObj;
    public string debugText;

    //comm
	public ServerComm ServerScript;

    //serverdaten
    public int gametime;
    public string state;

    //player control
    public int HumanPlayerID = -1;
    public Camera MainCam;
	public GameObject CamContainer;
	public GameObject SpectatorCam;

    // 0=food, 1=tech, 2=plant, 3=stuff/tools

	public Vector3[] islandCenterPoints;
    public Vector3[] startPoint; //drop off
   
    public List<Player> Players = new List<Player>();

    //items
    public List<Item> Items = new List<Item>();
    public int focusItem = -1;
	public int curItem = -1;
    public int pickId = -1;

    public int[] spawnPointList;

	public float radius = 3;
	Vector3 center;
	float angle;
	int itemCount;
    int rnd;
    int rndValue;

    float time = 0;
    public float updateTime = 1;

    // Use this for initialization
    void Start () {
        debugTextObj = GameObject.Find("debugText").GetComponent<TextMesh>();
        ServerScript = GetComponent<ServerComm>();

        MainCam = Camera.main;

		if (HumanPlayerID == -1) {
			SpectatorCam.SetActive (true);
			CamContainer.SetActive (false);
		} else {
			SpectatorCam.SetActive (false);
			CamContainer.SetActive (true);
		}

		if (HumanPlayerID == 0) SpawnItems();

        //init player position
        foreach (Player player in Players) {
            player.playerPos = startPoint[player.playerID];
        }
    }

    void Update() {

        debugTextObj.text = debugText;

        //timer 
        if (time < updateTime) time += Time.deltaTime;
        else {
			//print("update");

            // eigene position senden
			if (HumanPlayerID != -1) {
				ServerScript.SendPlayerPos (HumanPlayerID, Players[HumanPlayerID].playerPos, Players[HumanPlayerID].playerAngle);
			}

			time = 0;
        }
    }
  

    // 0-3 food
    // 4-7 tech
    // 8-11 plants
    // 12-15 stuff

	void SpawnItems() {
		Vector3 itemRingPos;

        // items im kreis um die holes verteilen
        for (int i = 0; i < 4; i++) {
            center = islandCenterPoints[i];

            for (int j = 0; j < 4; j++) {
                angle = j * 90 * Mathf.Deg2Rad;  //90° um das loch herumn
                float x = Mathf.Sin(angle) * radius;
                float y = Mathf.Cos(angle) * radius;
				itemRingPos = new Vector3(x,2,y) + center;

				// write parameters (cur island, homezone
				Items[spawnPointList[itemCount]].itemPos = itemRingPos;
				Items[spawnPointList[itemCount]].curIsland = i;
				Items[spawnPointList[itemCount]].updatePos = true;

				itemCount++;
            }
        }

        // Initialer Post der item positionen an den server
		for (int i=0; i<16; i++){
			if (Items[i].pickedById != -1) {
				ServerScript.UpdateItems (i, Items[i].itemPos, Items[i].curIsland, Items[i].pickedById);
			}
		}
    }

    public void tryPickObj(){
    print("try");
    	if (focusItem != -1) {
			Items[focusItem].pickedById = HumanPlayerID;
			curItem = focusItem;
			Items[focusItem].UpdateItem();
    	}
    }

    public void dropObj(){
		Items[curItem].pickedById = -1;
		curItem = -1;
    }
}


