using UnityEngine;
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
	public GameObject CamContainer;
	public GameObject SpectatorCam;

    // 0=food, 1=tech, 2=plant, 3=stuff/tools

	public Vector3[] islandCenterPoints;
    public Vector3[] startPoint; //drop off
   
    public List<Player> Players = new List<Player>();

    //items
    public List<Item> Items = new List<Item>();

    int[] spawnPointList = new int[16];

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

		if (HumanPlayerID == -1) {
			SpectatorCam.SetActive (true);
			CamContainer.SetActive (false);
		} else {
			SpectatorCam.SetActive (false);
			CamContainer.SetActive (true);
		}

		if (HumanPlayerID == 0) ShuffleItemSpawnPoints();

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

    void ShuffleItemSpawnPoints() {
        List<int> spawnIDs = new List<int>();
        List<int> spawnPoints = new List<int>();

        //pre-fill
        for (int i = 0; i < 16; i++) {
            spawnIDs.Add(i);
        }

        //shuffle
        for (int i = 0; i < 4; i++) { //anzahl inseln
            for (int j = 0; j < 4; j++) { //anzahl items pro insel

                //bool success = false;
                ////print("dddd");

                //do {

                //    // find value
                //    rnd = Random.Range(0, spawnIDs.Count); // 0-16 prefilled sorted int's
                //    rndValue = spawnIDs[rnd];

                //    // test value
                //    if (rndValue >= i * 4 && rndValue < i * 4 + 4) {
                //        success = false;
                //    } else {
                //        success = true;
                //        break;
                //    }
                //} while (!success);

                rnd = Random.Range(0, spawnIDs.Count); // 0-16 prefilled sorted int's
                rndValue = spawnIDs[rnd];

                //print(i + " chosen: " + rndValue);
                spawnPoints.Add(rndValue);
                spawnIDs.RemoveAt(rnd);
            }
        }

        //just for debugging
        foreach (int id in spawnPoints) {
            // print (id);
        }

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
				Items[spawnPoints[itemCount]].itemPos = itemRingPos;
				//Items[spawnPoints[itemCount]].zoneID = (int)spawnPoints [itemCount]/4;
				Items[spawnPoints[itemCount]].curIsland = i;
				Items[spawnPoints[itemCount]].updatePos = true;

				itemCount++;
            }
        }

        // Initialer Post der item positionen an den server
		for (int i=0; i<16; i++){
			if (Items[i].pickId != -1) {
				ServerScript.UpdateItems (i, Items[i].itemPos, Items[i].curIsland, Items[i].pickId);
			}
		}
    }
}


