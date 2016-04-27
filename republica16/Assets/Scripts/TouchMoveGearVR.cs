using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TouchMoveGearVR : MonoBehaviour {

	public Main MainScript;

	//sound
	public SoundFX SndScript;

    //public GameObject MainCam;

	public bool GearVRMode = false;
	//TextMesh debugText;

	bool mouseIsDown = false;
	bool doubleClick = false;
	public float doubleClickDelay = .5f;
	float time = 0;

	bool drown = false;

	public bool canJump = true;

	Rigidbody rb;
	public float jumpForce;
	public float force;

	float dirX = 0; 
	float dirZ = 0; 

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

        if (!GearVRMode) Move();
       
        if (Input.GetMouseButtonDown(0))
        {
            mouseIsDown = true;
            if (GearVRMode) Move ();
        }
        
		if (Input.GetMouseButtonUp (0)) {
			mouseIsDown = false;
			dirZ = dirX = 0;
		}

		//move while mouse down
		if (mouseIsDown) Move ();
		else dirZ = dirX = 0;

		// double click
		if (Input.GetMouseButtonDown (0) && !doubleClick) {
			doubleClick = true;
		} else if (Input.GetMouseButtonDown (0) && doubleClick && canJump) {
			PickDropObject ();
			doubleClick = false;
		}

		if (Input.GetButton("Fire1") && !doubleClick) {
			doubleClick = true;
		} else if (Input.GetButton("Fire1") && doubleClick && canJump) {
			PickDropObject ();
			doubleClick = false;
		}

		if (doubleClick) {
			if (time <= doubleClickDelay) {
				time += Time.deltaTime;
			} else {
				doubleClick = false;
				time = 0;
			}
		}

		//reset
		if (Input.GetKeyDown(KeyCode.Escape)) {
			SceneManager.LoadScene("titlescreen");
//			rb.velocity = Vector3.zero;
//			rb.angularVelocity = Vector3.zero;
//			transform.position = new Vector3 (1.6f, 1f, -11.3f);
//			transform.eulerAngles = Vector3.zero;
		}

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

		//canJump = false;
		//rb.AddForce (Vector3.up * jumpForce);
	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag == "Ground") {
			canJump = true;
			//print ("landed");
		}
	}

	void Move(){
		if (GearVRMode) {
			dirZ = (1280 - Input.mousePosition.x) / 300; //dirX = (720 - Input.mousePosition.y) / 300;
		} else {
			dirZ = Input.GetAxis ("Vertical");
		}

		Vector3 dir = Quaternion.Euler(new Vector3 (0,MainScript.MainCam.transform.localEulerAngles.y,0)) * Vector3.forward;
        transform.position += dirZ * dir * force;

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
    }
}
