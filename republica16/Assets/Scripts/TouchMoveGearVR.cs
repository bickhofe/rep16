using UnityEngine;
using System.Collections;

public class TouchMoveGearVR : MonoBehaviour {

    Camera MainCam;
	public bool GearVRMode = false;
	TextMesh debugText;

	bool mouseIsDown = false;
	bool doubleClick = false;
	public float doubleClickDelay = .5f;
	float time = 0;

	public bool canJump = true;

	Rigidbody rb;
	public float jumpForce;
	public float force;

	float dirX = 0; 
	float dirZ = 0; 

	// Use this for initialization
	void Start () {
        MainCam = Camera.main;
        debugText = GameObject.Find ("debugText").GetComponent<TextMesh> ();
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update (){

        //print ("mousedown: " + mouseIsDown);
        //print(MainCam.transform.localEulerAngles.y);
        //transform.localEulerAngles = new Vector3(0, MainCam.transform.localEulerAngles.y, 0);

        if (Input.GetMouseButtonDown (0)) {
			mouseIsDown = true;
			Move ();
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
			Jump ();
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
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			transform.position = new Vector3 (1.6f, 1f, -11.3f);
			transform.eulerAngles = Vector3.zero;

		}
			
	}
		
	void Jump(){
		canJump = false;
		rb.AddForce (Vector3.up * jumpForce);
	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag == "Ground")
		{
			canJump = true;
			print ("landed");
		}
	}

	void Move(){
		if (GearVRMode) {
			dirZ = (1280 - Input.mousePosition.x) / 300;
			//dirX = (720 - Input.mousePosition.y) / 300;
		} else {
			dirZ = Input.GetAxis ("Vertical");
			//dirX = Input.GetAxis ("Horizontal");
		}


        Vector3 dir = Quaternion.Euler(new Vector3 (0,MainCam.transform.localEulerAngles.y,0)) * Vector3.forward;
        transform.position += dirZ * dir * force;

        //transform.position += new Vector3 (dirX*force, 0, dirZ*force);
        //transform.position += Vector3.forward * dirZ*force;

        //debugText.text = " "+Input.mousePosition.y;
        debugText.text = dirZ+"\n"+dirX;
	}
}
