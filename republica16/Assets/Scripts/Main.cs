using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	public int HumanPlayerID = -1;
	public GameObject CamContainer;



    // 0=food, 1=tech, 2=plant, 3=stuff/tools

	public Vector3[] islandCenterPoints;

    public Vector3[] startPoint; //drop off

    public List<Player> players = new List<Player>();
    public Player[] Player;

    //items
    public List<Item> itemScripts = new List<Item>();
    public Item[] Items; 
    int[] spawnPointList = new int[16];

	public float radius = 3;
	Vector3 center;
	float angle;
	Vector3 itemPos;
	int itemCount;
    int rnd;
    int rndValue;

    float time = 0;
    public float updateTime = 1;

    // Use this for initialization
    void Start () {
        ShuffleItemSpawnPoints();

        for (int i = 0; i<Player.Length; i++) {
            players.Add(Player[i]);
        }
			
        // myList.RemoveAt(i);
        // print(players.Count);

        //init player position
        foreach (Player player in players)
        {
            player.playerPos = startPoint[player.playerID];
        }
    }

    void Update() {
        //timer
        if (time < updateTime) time += Time.deltaTime;
        else
        {
			//print("update");
            //UpdatePlayers();
            //UpdateItems();
        }
    }
	
	void UpdatePlayers() {
        
		foreach (Player data in players)
        {
			if (data.playerPos.y < -25) {
				data.ResetPlayer (startPoint [data.curIsland]);
			} else {
				if (data.playerID != HumanPlayerID) data.UpdatePlayer (); // human player available?
			}
        }

        time = 0;
    }

    void UpdateItems()
    {
		foreach (Item item in itemScripts)
        {
			if (item.itemPos.y < -25) item.ResetItem(startPoint[item.curIsland]);
			else item.UpdateItem();
        }

        time = 0;
    }

    // 0-4 food
    // 5-9 tech
    // 10-14 plants
    // 15-19 stuff

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
            //print (id);
        }


        for (int i = 0; i < 4; i++) {
            center = islandCenterPoints[i];

            for (int j = 0; j < 4; j++) {
                angle = j * 90 * Mathf.Deg2Rad;  //90° um das loch herumn
                float x = Mathf.Sin(angle) * radius;
                float y = Mathf.Cos(angle) * radius;
                itemPos = new Vector3(x, 2, y) + center;
                Items[spawnPoints[itemCount]].transform.position = itemPos;
                itemCount++;
            }
        }
    }
}


