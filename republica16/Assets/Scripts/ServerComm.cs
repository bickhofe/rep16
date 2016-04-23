using UnityEngine;
using System;
//using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class ServerComm : MonoBehaviour {
    // socket.io vars
	public static string userID;
	public static string userName = "UnityEngine";
	public static int port = 3000;
	public Main MainScript;
    private SocketIOComponent socket;

    // Use this for initialization
    void Start () {
        MainScript = GameObject.Find("Main").GetComponent<Main>();
		userID = ""+UnityEngine.Random.Range(0,1000);
		SocketIoConnect();
	}

    void SocketIoConnect() {
        // connect to socketIO
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        socket.On("onTick", GotTimetick);
		socket.On("pshuffled", GotCharacter);
        socket.On("shuffled", GotShuffled);
        socket.On("onItemRefresh", GotItems);
        socket.On("onPlayerPos", GotPlayerPosData);
    }

	void GotCharacter(SocketIOEvent inmsg) {
        Debug.Log("[SocketIO] Character data received: " + inmsg.name + " " + inmsg.data);
        JSONObject injson = inmsg.data as JSONObject;

        //neue id sequence senden
		MainScript.UpdateCharacterID(injson["shuffle"].str);
        //print ("Shuffle: " + injson["shuffle"].str);
    }


    void GotShuffled(SocketIOEvent inmsg) {
        Debug.Log("[SocketIO] Shuffle data received: " + inmsg.name + " " + inmsg.data);
        JSONObject injson = inmsg.data as JSONObject;
		MainScript.ProcessShuffleData(injson["shuffle"].str);
        //print ("Shuffle: " + injson["shuffle"].str);
    }

	void GotTimetick(SocketIOEvent inmsg) { //timer
        //Debug.Log("[SocketIO] Timer data received: " + inmsg.name + " " + inmsg.data);
        JSONObject injson = inmsg.data as JSONObject;
		MainScript.tempStatus = injson["state"].str; 
        //print ("State: " + injson["state"].str + "- tick " + injson["tick"].str );
       }

    //komplette item update (16mal) oder einzel update
    void GotItems(SocketIOEvent inmsg) {
       	//Debug.Log("[SocketIO] Item data received: " + inmsg.name + " " + inmsg.data);
		int tmpID = -1; float tmpX = 1; float tmpY = 1; float tmpZ = 1; int tmpIsland = -1; int tmpPickID = -1;

        JSONObject injson = inmsg.data as JSONObject;
		tmpID = int.Parse(injson["itemId"].str);
		tmpX = float.Parse(injson["itemPosX"].str);
		tmpY = float.Parse(injson["itemPosY"].str);
		tmpZ = float.Parse(injson["itemPosZ"].str);
		tmpIsland = int.Parse(injson["itemCurIsland"].str);
		tmpPickID = int.Parse(injson["itemPickId"].str);

		MainScript.Items[tmpID].itemPos = new Vector3 (tmpX,tmpY,tmpZ);
		MainScript.Items[tmpID].curIsland = tmpIsland;
		MainScript.Items[tmpID].pickedById = tmpPickID;

		MainScript.Items[tmpID].updatePos = true;

        //print("Item:->" + injson["itemId"].str +" CurIsland "+ injson["itemCurIsland"].str 
        //    + " PickID" + injson["itemCurIsland"].str + " x " + injson["itemPosX"].str + " y " 
        //    + injson["itemPosY"].str + " z " + injson["itemPosZ"].str);
    }

    void GotPlayerPosData(SocketIOEvent inmsg) {
        //Debug.Log("[SocketIO] Pos data received: " + inmsg.name + " " + inmsg.data);
        int tmpID = -1; float tmpX = 1; float tmpY = 1; float tmpZ = 1; float tmpAng = 0;

        JSONObject injson = inmsg.data as JSONObject;
        tmpID = int.Parse(injson["playerId"].str);
        tmpX = float.Parse(injson["playerPosX"].str);
        tmpY = float.Parse(injson["playerPosY"].str);
        tmpZ = float.Parse(injson["playerPosZ"].str);
        tmpAng = float.Parse(injson["playerAngle"].str);

        //// daten empfangen und an die playerscripte weiterleiten
        MainScript.Players[tmpID].playerPos = new Vector3 (tmpX,tmpY,tmpZ);
        MainScript.Players [tmpID].updatePos = true;
        //MainScript.debugText = tmpID.ToString();
    }

    void Update()
    {
        //space key pressed
        if (Input.GetKeyDown("space"))
        {
            //GetShuffle();
			//MainScript.UpdateCharacterID(1);
			GetCharacter();
			print("spac");
        }

        ////Mouse Click
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 testitempos = new Vector3(1.1f, 1.1f, 1.1f);
            //id, pos, cur island, user
            UpdateItems(12, testitempos, 1, 2);
        }

    }

	public void SendPlayerPos(int ID, Vector3 pos, float ang) {
       //print("send");
        Dictionary<string, string> message = new Dictionary<string, string>();
		message.Add("playerId", ID.ToString());
		message.Add("playerPosX", Math.Round(pos.x,2).ToString());
		message.Add("playerPosY", Math.Round(pos.y,2).ToString());
		message.Add("playerPosZ", Math.Round(pos.z,2).ToString());
		message.Add("playerAngle", ang.ToString());
        message.Add("playerHeadX", "1.0");
        message.Add("playerHeadY", "1.0");
        message.Add("playerHeadZ", "1.0");
        socket.Emit("onPlayerPos", new JSONObject(message));
    }

    //einzelnes items updaten
    public void UpdateItems(int ID, Vector3 pos, int curIsland, int pickedID) {
		//print(Math.Round(pos.x,2));
        Dictionary<string, string> message = new Dictionary<string, string>();
		message.Add("itemId", ID.ToString());
		message.Add("itemPosX", Math.Round(pos.x,2).ToString());
		message.Add("itemPosY", Math.Round(pos.y,2).ToString());
		message.Add("itemPosZ", Math.Round(pos.z,2).ToString());
		message.Add("itemCurIsland", curIsland.ToString());
		message.Add("itemPickId", pickedID.ToString());
        socket.Emit("onUpdateItem", new JSONObject(message));
    }

	public void GetCharacter() {
        Dictionary<string, string> message = new Dictionary<string, string>();
        message.Add("newshuffle", "character");
        socket.Emit("pshuffle", new JSONObject());
    }

    public void GetShuffle() {
        Dictionary<string, string> message = new Dictionary<string, string>();
        message.Add("newshuffle", "items");
        socket.Emit("shuffle", new JSONObject(message));
    }
}
