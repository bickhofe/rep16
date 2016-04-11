using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	public int HumanPlayerID = -1;
	public GameObject CamContainer;

    public Vector3[] startPoint;

    public List<Player> players = new List<Player>();
    public Player[] Player;

	public List<Item> items = new List<Item>();
    public Item[] Items;

    float time = 0;
    public float updateTime = 1;

    // Use this for initialization
    void Start () {

        for (int i = 0; i<Player.Length; i++) {
            players.Add(Player[i]);
        }
			
        // myList.RemoveAt(i);
        // print(players.Count);

        //init position
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
			print("update");
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
}


