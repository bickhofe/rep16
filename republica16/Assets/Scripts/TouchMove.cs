using UnityEngine;
using System.Collections;

public class TouchMove : MonoBehaviour {

	public Main MainScript;

	//sound
	public SoundFX SndScript;

	bool drown = false;

	Rigidbody rb;
	public float force;

	float dirX = 0; 
	float dirZ = 0; 
	public float walkSpeed = 0.2f;

	bool canPick = true;

	// Use this for initialization
	void Start () {
		MainScript = GameObject.Find("Main").GetComponent<Main>();

		SndScript = GameObject.Find("Environment").GetComponent<SoundFX>();

        //debugText = GameObject.Find ("debugText").GetComponent<TextMesh> ();
		rb = GetComponent<Rigidbody> ();

		// take control over
		if (MainScript.CharacterPlayerID != -1) {
			transform.position = MainScript.startPoint [MainScript.CharacterPlayerID];
		}
	}
	
	// Update is called once per frame
	void Update (){
		
		if (Input.touchCount > 0) {
			Vector3 touchPos = Input.GetTouch (0).position;

			// pick
			if (touchPos.x < Screen.width / 2) {
				if (canPick) {
					PickDropObject ();
					canPick = false;
				}
			}

			// walk
			if (touchPos.x > Screen.width / 2 && touchPos.x <= Screen.width) {
				dirZ = walkSpeed;
			}

//			if (Right.Contains (touchPos)) {
//				dirZ = walkSpeed;
//			}
		} else {
			canPick = true;
			dirZ = 0;
		}

		Move ();
	

		//fall from cliff
		if (transform.position.y < -10 && transform.position.y > -15 && !drown) {
			drown = true;
			SndScript.PlayAudio(SndScript.fallwater);
		}

		if (transform.position.y < -25) {
			TeleportPlayer(MainScript.startPoint[MainScript.Players[MainScript.CharacterPlayerID].curIsland]);
			SndScript.PlayAudio(SndScript.drown);
			drown = false;
		}


	}
		
	void PickDropObject() {
		//print ("pick/drop");
		if (MainScript.curItem == -1) { //player hat nichts in der hand
			MainScript.tryPickObj();
		} else {
			MainScript.dropObj();
		}
	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag == "Ground") {
			//xxx
		}
	}

	void Move(){
//		if (GearVRMode) {
//			dirZ = (1280 - Input.mousePosition.x) / 300; //dirX = (720 - Input.mousePosition.y) / 300;
//		} else {
//			//dirZ = Input.GetAxis ("Vertical");
//			dirZ = Input.GetAxis ("Horizontal")*-1;
//			if (Input.GetKey("space")) dirZ = .2f;
//		}

		Vector3 dir = Quaternion.Euler(new Vector3 (0,MainScript.MainCam.transform.localEulerAngles.y,0)) * Vector3.forward;
        transform.position += dirZ * dir * force;
		print (walkSpeed);

        //MainScript.debugText.text = dirZ+"\n"+dirX;
	}

	void OnTriggerEnter(Collider Portal) {
        print("Enter portal");
        int IslandID = Portal.transform.parent.GetComponent<Island>().IslandID;

		if (IslandID == 0) MainScript.Players[MainScript.CharacterPlayerID].curIsland = 3;
		else if (IslandID == 1) MainScript.Players[MainScript.CharacterPlayerID].curIsland = 2;
		else if (IslandID == 2) MainScript.Players[MainScript.CharacterPlayerID].curIsland = 0;
		else if (IslandID == 3) MainScript.Players[MainScript.CharacterPlayerID].curIsland = 1;

		TeleportPlayer(MainScript.startPoint[MainScript.Players[MainScript.CharacterPlayerID].curIsland]);

		SndScript.PlayAudio(SndScript.tuberide);
    }

    public void TeleportPlayer(Vector3 teleportPos) {
		//send player position updaten to server!
		rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = teleportPos;
        transform.eulerAngles = Vector3.zero;

		MainScript.GazeIslandText.text =  MainScript.islandNames[MainScript.Players[MainScript.CharacterPlayerID].curIsland]+" Island";
    }
}
