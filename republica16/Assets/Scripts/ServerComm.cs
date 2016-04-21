using UnityEngine;
using System;
//using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class ServerComm : MonoBehaviour {
  // pomelo vars
	public static string userID;
	public static string userName = "UnityEngine";
	public static string channel = "islands";
	public static string host = "104.155.72.59";
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
        socket.On("onArea", GotData);
        socket.On("onItemRefresh", GotItems);
        socket.On("onPlayerPos", GotPlayerPosData);
    }

    void GotData(SocketIOEvent inmsg) {
        JSONObject injson = inmsg.data as JSONObject;

        //System.Object state = null;
        //if (inmsg.TryGetValue("state", out state)) {
        //    print("-> " + state);
        //}

        //System.Object tick = null;
        //if (inmsg.TryGetValue("tick", out tick)) {
        //    //	print("-> " + tick);
        //    MainScript.debugText = tick.ToString();
        //}
    }

    void GotItems(SocketIOEvent inmsg) {
        JSONObject injson = inmsg.data as JSONObject;
        print("Item:->" + injson["itemId"].str +" CurIsland "+ injson["itemCurIsland"].str 
            + " PickID" + injson["itemCurIsland"].str + " x " + injson["itemPosX"].str + " y " 
            + injson["itemPosY"].str + " z " + injson["itemPosZ"].str);
        
    }

    void GotPlayerPosData(SocketIOEvent inmsg) {
        JSONObject injson = inmsg.data as JSONObject;
        //System.Object data = null;
		int tmpID = -1; float tmpX = 1; float tmpY = 1; float tmpZ = 1; float tmpAng = 0;
        tmpID = int.Parse(injson["playerId"].ToString());
        tmpX = int.Parse(injson["playerPosX"].ToString());
        tmpY = int.Parse(injson["playerPosY"].ToString());
        tmpZ = int.Parse(injson["playerPosZ"].ToString());
        tmpAng = int.Parse(injson["playerAngle"].ToString());

        //// daten empfangen und an die playerscripte weiterleiten
        MainScript.Players[tmpID].playerPos = new Vector3 (tmpX,tmpY,tmpZ);
        MainScript.Players [tmpID].updatePos = true;
        //MainScript.debugText = tmpID.ToString();
    }

    void Update()
    {
        //space key pressed
        //if (Input.GetKeyDown("space")) {
        //SendPlayerPos();
        //}

        ////Mouse Click
        //if (Input.GetMouseButtonDown(0)){
        //	SendServer("Clicked","server");
        //}

    }

	public void SendPlayerPos(int ID, Vector3 pos, float ang) {
       //print("send");
        Dictionary<string, string> message = new Dictionary<string, string>();
        message.Add("area", channel);
		message.Add("playerId", ID.ToString());
		message.Add("playerPosX", pos.x.ToString());
		message.Add("playerPosY", pos.y.ToString());
		message.Add("playerPosZ", pos.z.ToString());
		message.Add("playerAngle", ang.ToString());
        message.Add("playerHeadX", "1.0");
        message.Add("playerHeadY", "1.0");
        message.Add("playerHeadZ", "1.0");
        message.Add("from", userName);
        message.Add("target", "*"); // * alle in der area
        socket.Emit("onPlayerPos", new JSONObject(message));
    }

    public void UpdateItems(int ID, Vector3 pos, int curIsland, int pickedID) {
        Dictionary<string, string> message = new Dictionary<string, string>();
        message.Add("area", channel);
		message.Add("itemId", ID.ToString());
		message.Add("itemPosX", pos.x.ToString());
		message.Add("itemPosY", pos.y.ToString());
		message.Add("itemPosZ", pos.z.ToString());
		message.Add("itemCurIsland", curIsland.ToString());
		message.Add("itemPickId", pickedID.ToString());
		message.Add("from", userName);
        socket.Emit("onUpdateItem", new JSONObject(message));
    }
}
