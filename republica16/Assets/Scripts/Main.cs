using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Main : MonoBehaviour {

    //debug
    public TextMesh debugTextObj;
    public string debugText;

    //hud
	Text GazeTimeText;
	Text GazeMsgText;
	Text GazeIslandText;

    //comm
	public ServerComm ServerScript;

	//sound
	public SoundFX SndScript;

    //serverdaten
    public int gametime;
    public string state;

    //player control
    public int DeviceId;
    public int CharacterPlayerID = -1;
    public Camera MainCam;
	public GameObject CamContainer;
	public GameObject SpectatorCam;

    public string[] islandNames;

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
		DeviceId = PlayerPrefs.GetInt("DeviceID");

        debugTextObj = GameObject.Find("debugText").GetComponent<TextMesh>();
        ServerScript = GetComponent<ServerComm>();

		SndScript = GameObject.Find("Environment").GetComponent<SoundFX>();

        MainCam = Camera.main;
		CamContainer.SetActive (false);
		GazeTimeText = GameObject.Find("hudTime").GetComponent<Text>();
		GazeMsgText = GameObject.Find("hudMessage").GetComponent<Text>();
		GazeIslandText = GameObject.Find("hudIsland").GetComponent<Text>();

		GazeIslandText.text =  "You are here:\n"+islandNames[Players[CharacterPlayerID].curIsland]+" Island";
    }

    void Update() {

        if (tempStatus != gameStatus) {
        	gameStatus = tempStatus;

        	//fragt initial UND bei wechsel von pause zu running einmal die neuen shuffled liste an
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

	public void UpdateTime(string time){
		print("stat"+gameStatus);
		if (gameStatus == "paused") {
			GazeTimeText.text =  "Pause: "+time;
			GazeMsgText.text =  "Please wait...";
		} else {
			GazeTimeText.text =  "Time: "+time;
			GazeMsgText.text =  "Bring all "+islandNames[CharacterPlayerID]+" items to your island!";
		}

		//GazeMsgText =  "";
    }

    public void PlayerWon(string playerId){
    	print("Player"+playerId+ "won!");
    }

    public void UpdateCharacterID(string idlist){

		string[] ids = idlist.Split(',');
		CharacterPlayerID = int.Parse(ids[DeviceId]);
		Players[CharacterPlayerID].InitMonster();

		//switch to specatror cam
		if (DeviceId == -1) {
			print("spec");
			SpectatorCam.SetActive (true);
			CamContainer.SetActive (false);
		} else {
			print("player");
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
			SndScript.PlayAudio(SndScript.pick);
    	}
    }

    public void dropObj(){
		Items[curItem].pickedById = -1;
		curItem = -1;
		SndScript.PlayAudio(SndScript.drop);
    }
}


