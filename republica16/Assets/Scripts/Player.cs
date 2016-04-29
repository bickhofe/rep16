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
	Vector3 camRotation;

	bool restart = true;
	GameObject Monster;

	Collider col;

    // Use this for initialization
    void Start () {
        MainScript = GameObject.Find("Main").GetComponent<Main>();
		col = GetComponent<Collider>();
		Monster = transform.Find("Character/Monster").gameObject;

		Monster.SetActive(false); //macht den eigenen playercharacter unsichtbar
        curIsland = playerID;
    }

	public void InitMonster(){
		Monster.SetActive(false);
	}
	
	void Update() {
		if (MainScript.CharacterPlayerID == playerID) {
			//position von camera holen
			transform.position = MainScript.CamContainer.transform.position;
			camRotation = MainScript.MainCam.transform.eulerAngles;
			transform.eulerAngles = new Vector3(0, camRotation.y, 0); 

			//player pos parameter setzen
			playerPos = MainScript.CamContainer.transform.position;
			playerAngle = camRotation.y;
		} else {
			if (updatePos) {
				print(playerID);
				Monster.SetActive(true);
				transform.position = playerPos;
				transform.eulerAngles = new Vector3(0, playerAngle, 0); 
				updatePos = false;
			}
		}
    }
}
