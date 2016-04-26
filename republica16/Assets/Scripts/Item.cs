using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    public Main MainScript;

	//sound
	public SoundFX SndScript;

    public int itemID;
    //public int zoneID;
    public Vector3 itemPos;
    public int curIsland;
	public int pickedById = -1;
	public bool updatePos = false;

	Rigidbody rb;

    // Use this for initialization
    void Start() {
        MainScript = GameObject.Find("Main").GetComponent<Main>();
		rb = GetComponent<Rigidbody>();

		SndScript = GameObject.Find("Environment").GetComponent<SoundFX>();
    }

    void Update() {
		if (updatePos) {
			transform.position = itemPos;
			updatePos = false;
		}

		if (pickedById != -1) {
			print("dd");
			rb.useGravity = false;
			Transform CamTransform =  MainScript.MainCam.transform;
			transform.position = new Vector3(CamTransform.position.x,1,CamTransform.position.z) + CamTransform.forward*2;
			if (transform.localPosition.y < -6) transform.localPosition = new Vector3(transform.localPosition.x,-6,transform.localPosition.z);

		} else {
			rb.useGravity = true; 
		}

		//fall from cliff
		if (transform.position.y < -25) TeleportItem(MainScript.startPoint[curIsland]);
    }

	//update aufruf von main script
	/// rumtragen vom 3sek updaten ausschliessen!!! zeile 25!!
	public void UpdateItem() {
		if (pickedById == -1) transform.position = itemPos;
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
		SndScript.PlayAudio(SndScript.senditem);
	}

    public void TeleportItem(Vector3 teleportPos) {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = teleportPos;
		itemPos = transform.position;
        transform.eulerAngles = Vector3.zero;

		//send item position update to server!
		MainScript.ServerScript.UpdateItems (itemID, itemPos, curIsland, pickedById);
    }
}
