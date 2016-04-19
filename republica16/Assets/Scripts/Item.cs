using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    public Main MainScript;

    public int itemID;
    //public int zoneID;
    public Vector3 itemPos;
    public int curIsland;
	public int pickId = -1;
	public bool updatePos = false;

	Rigidbody rb;

    // Use this for initialization
    void Start() {
        MainScript = GameObject.Find("Main").GetComponent<Main>();
		rb = GetComponent<Rigidbody>();
    }

    void Update() {
		if (updatePos) {
			transform.position = itemPos;
			updatePos = false;
		}
    }

	//update aufruf von main script
	public void UpdateItem() {
		transform.position = itemPos;
	}

    void OnTriggerEnter(Collider Portal) {
		print ("Enter portal");

		// ID der Insel des Colliders finden
		int IslandID = Portal.transform.parent.GetComponent<Island> ().IslandID;

		if (IslandID == 0)
			curIsland = 3;
		else if (IslandID == 1)
			curIsland = 2;
		else if (IslandID == 2)
			curIsland = 0;
		else if (IslandID == 3)
			curIsland = 1;

		TeleportItem (MainScript.startPoint [curIsland]);
	}

    public void TeleportItem(Vector3 teleportPos) {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = teleportPos;
        transform.eulerAngles = Vector3.zero;
    }
}
