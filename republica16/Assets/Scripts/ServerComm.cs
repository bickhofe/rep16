using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;
using Pomelo.DotNetClient;

// http://2k4.de:3001/?area=islands&player=-1;


public class ServerComm : MonoBehaviour {
  // pomelo vars
	public static string userID;
	public static string userName = "UnityEngine";
	public static string channel = "islands";
	public static string host = "104.155.72.59";
	public static int connectorport = 3014;
	public static JsonObject users = null;
	public static PomeloClient pc = null;
	private ArrayList userList = null;
	public Main MainScript;

    // Use this for initialization
    void Start () {
	    MainScript = GameObject.Find("Main").GetComponent<Main>();
	    userList = new ArrayList();
		userID = ""+UnityEngine.Random.Range(0,1000);
		Login();
	}

    void Login() {
        pc = new PomeloClient(host, connectorport);
        pc.connect(null, (data) => {
            JsonObject msg = new JsonObject();
			msg["uid"] = userID;
            pc.request("gate.gateHandler.queryEntry", msg, OnQuery);
        });
    }

    // Reconnect to Game Server with the Connection Date we got from Login
    void OnQuery(JsonObject result)
    {
        if (Convert.ToInt32(result["code"]) == 200)
        {
            pc.disconnect();
            string host = (string)result["host"];
            int port = Convert.ToInt32(result["port"]);
            pc = new PomeloClient(host, port);

            pc.connect(null, (data) => {
                GetOtherPlayers();
            });

            // area msg
            pc.on("onArea", (data) => {
                GotData(data);
            });

            // area msg
            pc.on("onItemRefresh", (data) => {
                // TBD
                print("Itemdata refreshed!");
                //
            });

            // player Pos update
            pc.on("onPlayerPos", (data) => {
                GotPlayerPosData(data);
            });

            //neuer spieler
            pc.on("onAdd", (data) => {
                RefreshUserList("add", data);
            });

            //spieler verl�sst den server
            pc.on("onLeave", (data) => {
                RefreshUserList("leave", data);
            });
        }
    }

    //Request List of other Players
    void GetOtherPlayers()
    {
        JsonObject userMessage = new JsonObject();
        userMessage.Add("username", userName);
        userMessage.Add("area", channel);
        if (pc != null)
        {
            pc.request("connector.entryHandler.enter", userMessage, (data) => {
                users = data;
                print("user"+users.ToString());
            });
        }
    }

    void GotData(JsonObject inmsg) {
        System.Object state = null;
        if (inmsg.TryGetValue("state", out state)) {
            print("-> " + state);
        }

        System.Object tick = null;
        if (inmsg.TryGetValue("tick", out tick)) {
            //	print("-> " + tick);
            MainScript.debugText = tick.ToString();
        }
    }

    void GotPlayerPosData(JsonObject inmsg) {
        print(inmsg.ToString());

        System.Object data = null;
		int tmpID = -1; float tmpX = 1; float tmpY = 1; float tmpZ = 1; float tmpAng = 0;

		if (inmsg.TryGetValue("playerId", out data)) {
			tmpID = int.Parse(""+data);

			inmsg.TryGetValue("playerPosX", out data);
			tmpX = float.Parse(""+data);

			inmsg.TryGetValue("playerPosY", out data);
			tmpY = float.Parse(""+data);

			inmsg.TryGetValue("playerPosZ", out data);
			tmpZ = float.Parse(""+data);

			inmsg.TryGetValue("playerAngle", out data);
			tmpAng = float.Parse(""+data);
		}

		// daten empfangen und an die playerscripte weiterleiten
		MainScript.Players[tmpID].playerPos = new Vector3 (tmpX,tmpY,tmpZ);
		MainScript.Players [tmpID].updatePos = true;
        //MainScript.debugText = tmpID.ToString();
    }


    //Update the userlist.
    void RefreshUserList(string flag,JsonObject msg) {
		System.Object user = null;
		if(msg.TryGetValue("user", out user)) {
			if (flag == "add") {
				this.userList.Add(user.ToString());
			} else if (flag == "leave") {
				this.userList.Remove(user.ToString());
			}
			print("User "+flag+" "+user.ToString());
		}
	}

	void Update () {

        // esc key pressed
        if (Input.GetKey(KeyCode.Escape)) {
            //if (pc != null) {
                //pc.disconnect();
            //}
            //Application.Quit();
        }

        //space key pressed
        if (Input.GetKeyDown("space")) {
	      //SendPlayerPos();
	    }

		////Mouse Click
		//if (Input.GetMouseButtonDown(0)){
		//	SendServer("Clicked","server");
		//}
	}

    void OnApplicationQuit() {
        if (pc != null) {
           pc.disconnect();
        }
    }

        // Send klick Msg to Area
    void SendAll(JsonObject msg) {
        JsonObject message = new JsonObject();
        message.Add("area", channel);
        message.Add("content", msg);
        message.Add("from", userName);
        message.Add("target", "*"); // * alle in de area

        if (pc != null) {
            pc.request("pdg.pdgHandler.send", message, (data) => {
                // print(data);
            });
        }
    }

	public void SendPlayerPos(int ID, Vector3 pos, float ang) {
        //print("send");

    JsonObject message = new JsonObject();
    message.Add("area", channel);
		message.Add("playerId", ID);
		message.Add("playerPosX", pos.x);
		message.Add("playerPosY", pos.y);
		message.Add("playerPosZ", pos.z);
		message.Add("playerAngle", ang);
    message.Add("playerHeadX", "1.0");
    message.Add("playerHeadY", "1.0");
    message.Add("playerHeadZ", "1.0");
    message.Add("from", userName+ID);
    message.Add("target", "*"); // * alle in der area

        if (pc != null) {
            pc.request("pdg.pdgHandler.sendplayerpos", message, (data) => {
                //print("data"+data);
            });
        } else {
          print("Connection error!");
        }
    }

    public void UpdateItems(int ID, Vector3 pos, int curIsland, int pickedID) {
      JsonObject message = new JsonObject();
      message.Add("area", channel);
  		message.Add("itemId", ID);
  		message.Add("itemPosX", pos.x);
  		message.Add("itemPosY", pos.y);
  		message.Add("itemPosZ", pos.z);
  		message.Add("itemCurIsland", curIsland);
  		message.Add("itemPickId", pickedID);
  		message.Add("from", userName+ID);
      message.Add("target", "*"); // * alle in der area

        if (pc != null) {
            pc.request("pdg.pdgHandler.updateitempos", message, (data) => {
                // print(data);
            });
        } else {
          print("Connection error!");
        }
    }

    // Send klick Msg only to "server" or other "playername"
    void SendServer (string msg, string target) {
		JsonObject message = new JsonObject();
		message.Add("area", channel);
		message.Add("content", msg);
		message.Add("from", userName);
		message.Add("target", target); // an "server" oder anderer "spielername"

        if (pc != null) {
			pc.request("pdg.pdgHandler.send", message, (data) => {
				print("Only to Server");
			});
		}
	}


}
