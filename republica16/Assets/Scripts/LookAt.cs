using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LookAt : MonoBehaviour {

	public Main MainScript;

	RectTransform Gaze;
	Text GazeText;

	void Start(){
		MainScript = GameObject.Find("Main").GetComponent<Main>();

		Gaze = GameObject.Find("Gaze").GetComponent<RectTransform>();
		GazeText = GameObject.Find("GazeText").GetComponent<Text>();
	}

    void Update() {

        //debug
		Vector3 forward = MainScript.MainCam.transform.TransformDirection(Vector3.forward) * 10;
		Debug.DrawRay(MainScript.MainCam.transform.position, forward, Color.green);

        RaycastHit hit;
		if (Physics.Raycast (MainScript.MainCam.transform.position, MainScript.MainCam.transform.forward, out hit, 4)) {
			
			print ("hit: " + hit.collider.name);

			if (hit.collider.tag == "Item") {
				print ("Item found!");

				Gaze.sizeDelta = new Vector2 (15, 15);

				// get temp itemscript
				Item ItemScript;

				if (MainScript.focusItem == -1) {
					ItemScript = hit.collider.GetComponent<Item> ();
					MainScript.focusItem = ItemScript.itemID;
					GazeText.text = hit.collider.name+": "+ItemScript.itemID + " " +ItemScript.curIsland;
				}

			} else {
				//hit.collider.GetComponent<Item> ().pickedByPlayerID = -1;
				Gaze.sizeDelta = new Vector2 (25,25);
				GazeText.text = "";
				MainScript.focusItem = -1;
			}
		} 

    }
}
