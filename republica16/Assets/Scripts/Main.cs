using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

    //debug
    public TextMesh debugTextObj;
    public string debugText;

    //comm
	public ServerComm ServerScript;

    //serverdaten
    public int gametime;
    public string state;

    //player control
    public int DeviceId;
    public int CharacterPlayerID = -1;
    public Camera MainCam;
	public GameObject CamContainer;
	public GameObject SpectatorCam;

    // 0=food, 1=tech, 2=plant, 3=stuff/tools

	public Vector3[] islandCenterPoints;
    public Vector3[] startPoint; //drop off
   
    public List<Player> Players = new List<Player>();

    //items
    public List<Item> Items = new List<Item>();
    public int focusItem = -1;
	public int curItem = -1;
    public int pickId = -1;

    //spielstatus
	public string tempStatus;
    public string gameStatus = ""; 

    //inital item positionen
	public int[] spawnList;

	public float radius = 3;
	Vector3 center;
	float angle;
	int itemCount;
    int rnd;
    int rndValue;

    float time = 0;
    public float updateTime = 1;

    // Use this for initialization
    void Start () {
        debugTextObj = GameObject.Find("debugText").GetComponent<TextMesh>();
        ServerScript = GetComponent<ServerComm>();

        MainCam = Camera.main;
		CamContainer.SetActive (false);
    }

    void Update() {

        debugTextObj.text = debugText;

        if (tempStatus != gameStatus) {
        	gameStatus = tempStatus;

        	//fragt bei wechsel von pause zu running einmal die neuen shuffled liste an
        	if (gameStatus == "running") {
        		print("order shuffle");
        		ServerScript.GetCharacter();
				ServerScript.GetShuffle();
        	}
        }


        //timer 
        if (time < updateTime) time += Time.deltaTime;
        else {
			//print("update");

            // eigene position senden
			if (CharacterPlayerID != -1) {
				ServerScript.SendPlayerPos (CharacterPlayerID, Players[CharacterPlayerID].playerPos, Players[CharacterPlayerID].playerAngle);
			}

			time = 0;
        }
    }

    public void UpdateCharacterID(string idlist){

		string[] ids = idlist.Split(',');
		CharacterPlayerID = int.Parse(ids[DeviceId]);

		if (CharacterPlayerID == -1) {
			SpectatorCam.SetActive (true);
			CamContainer.SetActive (false);
		} else {
			SpectatorCam.SetActive (false);
			CamContainer.SetActive (true);
		}

        //init player position
        foreach (Player player in Players) {
            player.playerPos = startPoint[player.playerID];
        }
    }

    // 0-3 food
    // 4-7 tech
    // 8-11 plants
    // 12-15 stuff

	public void ProcessShuffleData(string data){
		spawnList = new int[16];

		int count = 0;
		foreach(var s in data.Split(',')) {
	        int num;
	        if (int.TryParse(s, out num)){
				spawnList[count] = num;
				count++;
	        }			
	    }

	    SpawnItems();
	}

	void SpawnItems() {
		Vector3 itemRingPos;

        // items im kreis um die holes verteilen
		itemCount = 0;

        for (int i = 0; i < 4; i++) {
            center = islandCenterPoints[i];

            for (int j = 0; j < 4; j++) {
                angle = j * 90 * Mathf.Deg2Rad;  //90° um das loch herumn
                float x = Mathf.Sin(angle) * radius;
                float y = Mathf.Cos(angle) * radius;
				itemRingPos = new Vector3(x,2,y) + center;

				// write parameters (cur island, homezone
				Items[spawnList[itemCount]].itemPos = itemRingPos;
				Items[spawnList[itemCount]].curIsland = i;
				Items[spawnList[itemCount]].updatePos = true;

				itemCount++;
            }
        }

        // Initialer Post der item positionen an den server
		for (int i=0; i<16; i++){				
			ServerScript.UpdateItems (i, Items[i].itemPos, Items[i].curIsland, Items[i].pickedById);
		}
    }

    public void tryPickObj(){
    print("try");
    	if (focusItem != -1) {
			Items[focusItem].pickedById = CharacterPlayerID;
			curItem = focusItem;
			Items[focusItem].UpdateItem();
    	}
    }

    public void dropObj(){
		Items[curItem].pickedById = -1;
		curItem = -1;
    }
}


