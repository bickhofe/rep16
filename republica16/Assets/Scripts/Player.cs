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
	public bool updatePos = false;

	Collider col;

    // Use this for initialization
    void Start () {
        MainScript = GameObject.Find("Main").GetComponent<Main>();
		col = GetComponent<Collider>();

		if (playerID != MainScript.HumanPlayerID) {
			transform.Find("Character").GetComponent<Renderer>().material.color = _color;
		} else {
			//eigenen character unsichtbar machen
			transform.Find("Character").gameObject.SetActive(false);
		}

        curIsland = playerID;
    }
	
	void Update() {
		if (MainScript.HumanPlayerID == playerID) {
			//position von camera holen
			transform.position = MainScript.CamContainer.transform.position;

			//player pos parameter setzen
			playerPos = MainScript.CamContainer.transform.position;
		} else {
			if (updatePos) {
				transform.position = playerPos;
				updatePos = false;
			}
		}
    }
}
