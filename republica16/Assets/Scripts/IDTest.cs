using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IDTest : MonoBehaviour {
    
    //items
    public List<Item> items = new List<Item>();
    int rnd;
    int rndValue;

    // Use this for initialization
    void Start() {
        ShuffleItemSpawnPoints();
    }

    void ShuffleItemSpawnPoints() {
        List<int> spawnIDs = new List<int>();
        List<int> spawnPoints = new List<int>();

        //pre-fill
        for (int i = 0; i < 16; i++) {
            spawnIDs.Add(i);
        }

        //shuffle
        for (int i = 0; i < 4; i++) { //ammount islands
            for (int j = 0; j < 4; j++) { //ammount items per islands

                bool success = false;

                do {
                    // find value
                    rnd = Random.Range(0, spawnIDs.Count); // 0-16 prefilled sorted int's
                    rndValue = spawnIDs[rnd];

                    // test value
                    if (rndValue >= i * 4 && rndValue < i * 4 + 4) {
                        success = false;
                    } else {
                        success = true;
                        break;
                    }
                } while (!success);

                spawnPoints.Add(rndValue);
                spawnIDs.RemoveAt(rnd);
            }
        }

        //just for debugging
        foreach (int id in spawnPoints) {
            print (id);
        }
    }
}