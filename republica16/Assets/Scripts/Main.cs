using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

    List<Player> players = new List<Player>();    // a real-world example of declaring a List of 'GameObjects'
    public Player Player0;
    public Player Player1;
    public Player Player2;
    public Player Player3;

    // Use this for initialization
    void Start () {

        players.Add(Player0);
        players.Add(Player1);
        players.Add(Player2);
        players.Add(Player3);                                      // add an item to the end of the List
        //myList[i] = newItem;                                  // change the value in the List at position i
        //Type thisItem = List[i];                              // retrieve the item at position i
        //myList.RemoveAt(i);
        print(players.Count);

        foreach(Player data in players)
        {
            print("player: " + data);
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}


