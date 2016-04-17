using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Main MainScript;

    public int playerID;
    public Vector3 playerPos;
    public float playerAngle;
    public Vector3 playerHead;
    public Color _color;
    public int curIsland;

    Rigidbody rb;
	Collider col;

    // Use this for initialization
    void Start () {
        MainScript = GameObject.Find("Main").GetComponent<Main>();
        rb = GetComponent<Rigidbody>();
		col = GetComponent<Collider>();
        transform.Find("Cube").GetComponent<Renderer>().material.color = _color;
        curIsland = playerID;

		//deactivate rigidbody when on user control
		if (MainScript.HumanPlayerID == playerID) {
			rb.isKinematic = false;
			col.enabled = false;
		}
    }
	
	void Update() {
		if (MainScript.HumanPlayerID == playerID) {
			transform.position = MainScript.CamContainer.transform.position;
		}

        //var rnd = Random.Range(-.1f, .1f);
        //playerPos = transform.position + new Vector3(rnd, 0, rnd);

        playerPos = transform.position;
    }

    void OnTriggerEnter(Collider Portal)
    {
        print("Enter portal");
        int IslandID = Portal.transform.parent.GetComponent<Island>().IslandID;

        if (IslandID == 0) curIsland = 3;
        else if (IslandID == 1) curIsland = 2;
        else if (IslandID == 2) curIsland = 0;
        else if (IslandID == 3) curIsland = 1;

        TeleportPlayer(MainScript.startPoint[curIsland]);
    }

    public void UpdatePlayer () {
        transform.position = playerPos;
    }

    public void TeleportPlayer(Vector3 teleportPos)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = teleportPos;
        transform.eulerAngles = Vector3.zero;
    }

    public void ResetPlayer(Vector3 startPos)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPos;
        transform.eulerAngles = Vector3.zero; 
    }
}
