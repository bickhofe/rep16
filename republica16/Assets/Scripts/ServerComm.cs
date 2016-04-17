using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;
using Pomelo.DotNetClient;



public class ServerComm : MonoBehaviour {
    // http://2k4.de:3001/?area=islands&player=-1;
    // pomelo vars
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
		Login();
	}

    void Login()
    {
        pc = new PomeloClient(host, connectorport);
        pc.connect(null, (data) => {
            JsonObject msg = new JsonObject();
            msg["uid"] = userName;
            pc.request("gate.gateHandler.queryEntry", msg, OnQuery);
        });
    }

    // Reconnect to Game Server
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
                print(users.ToString());
            });
        }
    }

    void GotData(JsonObject inmsg)
    {
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

    void GotPlayerPosData(JsonObject inmsg)
    {
      print(inmsg.ToString());
      System.Object playerId = null;
      if (inmsg.TryGetValue("playerId", out playerId))
      {
          print("-> " + playerId.ToString());
      }
    }


    //Update the userlist.
    void RefreshUserList(string flag,JsonObject msg){
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

        //esc key pressed
  //      if (Input.GetKey(KeyCode.Escape)) {
		//	if (pc != null) {
		//		//pc.disconnect();
		//	}
		//	Application.Quit();
		//}

		//space key pressed
		if (Input.GetKeyDown("space")) {

            JsonObject playerdata = new JsonObject();
            playerdata.Add("playerId", "1");
            playerdata.Add("playerPosX", "1.0");
            playerdata.Add("playerPosY", "1.0");
            playerdata.Add("playerPosZ", "1.0");
            playerdata.Add("playerAngle", "45");
            playerdata.Add("playerHeadX", "1.0");
            playerdata.Add("playerHeadY", "1.0");
            playerdata.Add("playerHeadZ", "1.0");

            //print(playerdata);

            SendPlayerPos(playerdata);
        }

		////Mouse Click
		//if (Input.GetMouseButtonDown(0)){
		//	SendServer("Clicked","server");
		//}
	}

    void OnApplicationQuit() {
        // if (pc != null) {
           pc.disconnect();
        // }
    }

        // Send klick Msg to Area
    void SendAll(JsonObject msg)
    {
        JsonObject message = new JsonObject();
        message.Add("area", channel);
        message.Add("content", msg);
        message.Add("from", userName);
        message.Add("target", "*"); // * alle in de area

        if (pc != null)
        {
            pc.request("pdg.pdgHandler.send", message, (data) => {
                // print(data);
            });
        }
    }
    void SendPlayerPos(JsonObject msg)
    {
        JsonObject message = new JsonObject();
        message.Add("area", channel);
        message.Add("playerId", msg.playerId);
        message.Add("playerPosX", msg.playerPosX);
        message.Add("playerPosY", msg.playerPosY);
        message.Add("playerPosZ", msg.playerPosZ);
        message.Add("playerAngle", msg.playerAngle);
        message.Add("playerHeadX", msg.playerHeadX);
        message.Add("playerHeadY", msg.playerHeadY);
        message.Add("playerHeadZ", msg.playerHeadZ);
        message.Add("from", userName);
        message.Add("target", "*"); // * alle in de area

        if (pc != null)
        {
            pc.request("pdg.pdgHandler.sendplayerpos", message, (data) => {
                // print(data);
            });
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
