using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	public int HumanPlayerID = -1;
	public GameObject CamContainer;

    Vector3 center0 = new Vector3(0, 0, -10);
    Vector3 center1 = new Vector3(-10, 0, 0);
    Vector3 center2 = new Vector3(0, 0, 10);
    Vector3 center3 = new Vector3(10, 0, 0);

    public Vector3[] startPoint; //drop off

    public List<Player> players = new List<Player>();
    public Player[] Player;

    //items
    public List<Item> items = new List<Item>();
    public Item[] Items;
    int[] spawnPointList = new int[20];

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
        foreach (Player data in players)
        {
            data.playerPos = startPoint[data.playerID];
        }
    }

    void Update() {
        //timer
        if (time < updateTime) time += Time.deltaTime;
        else
        {
			//print("update");
            UpdatePlayers();
            UpdateItems();
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
        foreach (Item data in items)
        {
            if (data.itemPos.y < -25) data.ResetItem(startPoint[data.curIsland]);
            else data.UpdateItem();
        }

        time = 0;
    }

    // 0-4 food
    // 5-9 tech
    // 10-14 plants
    // 15-19 tools

    void ShuffleItemSpawnPoints() {
        List<int> spawnIDs = new List<int>();
        List<int> spawnPoints = new List<int>();

        //pre-fill
        for (int i = 0; i < 20; i++) {
            spawnIDs.Add(i);
        }

        //shuffle
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 5; j++) {
                int rnd = Random.Range(0, spawnIDs.Count);
                while (rnd >= i * 5 && rnd < i * 5 + 5) rnd = Random.Range(0, spawnIDs.Count);
                spawnPoints.Add(spawnIDs[rnd]);
                spawnIDs.RemoveAt(rnd);
            }
        }

        //just for debugging
        foreach (int id in spawnPoints) {
            //print (id);
        }

        PlaceItems();
    }

    void PlaceItems() {
        //int radius = 2;

        //for (int i = 0; i < 5; i++) {

        //    // "i" now represents the progress around the circle from 0-1
        //    // we multiply by 1.0 to ensure we get a fraction as a result.
        //    float step = i / 5;
        //    // get the angle for this step (in radians, not degrees)
        //    float angle = step * Mathf.PI * 2;
        //    // the X &amp; Y position for this angle are calculated using Sin &amp; Cos
        //    float x = Mathf.Sin(angle) * radius;
        //    float z = Mathf.Cos(angle) * radius;

        //    Vector3 pos = new Vector3(x,0,z) + center0;

        //    Items[i].itemPos = pos;
        //    Items[i].UpdateItem();
        //}


        //code to try

        //Vector3 RandomCircle (Vector3 center, float radius){
        //    Vector3 ang = Random.value * 360;
        //    Vector3 pos;
        //    pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        //    pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        //    pos.z = center.z;
        //    return pos;
        //}
        
        //     Vector3 center = transform.position;
        //for (i = 0; i < numObjects; i++) {
        //    Vector3 pos = RandomCircle(center, 10);
        //    Vector3 rot = Quaternion.FromToRotation(Vector3.forward, center - pos);
        //    Instantiate(prefab, pos, rot);
        //}


    }
}


