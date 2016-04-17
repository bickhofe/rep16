using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LookAt : MonoBehaviour {

	public Main MainScript;

	RectTransform Gaze;
	Text GazeText;

    public GameObject VRCamHead;
    // Update is called once per frame

	void Start(){
		MainScript = GameObject.Find("Main").GetComponent<Main>();

		Gaze = GameObject.Find("Gaze").GetComponent<RectTransform>();
		GazeText = GameObject.Find("GazeText").GetComponent<Text>();
	}

    void Update() {
        //debug
        Vector3 forward = VRCamHead.transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(VRCamHead.transform.position, forward, Color.green);

        RaycastHit hit;
		if (Physics.Raycast (VRCamHead.transform.position, VRCamHead.transform.forward, out hit)) {
			
			print ("hit: " + hit.collider.name);

			if (hit.collider.tag == "Item") {
				print ("Item found!");

				Gaze.sizeDelta = new Vector2 (15, 15);
				hit.collider.GetComponent<Item> ().pickedByPlayerID = MainScript.HumanPlayerID;

				GazeText.text = hit.collider.name+": "+hit.collider.GetComponent<Item> ().itemID + " " +hit.collider.GetComponent<Item> ().curIsland+ " " +hit.collider.GetComponent<Item> ().zoneID;
			} else {
				//hit.collider.GetComponent<Item> ().pickedByPlayerID = -1;
				Gaze.sizeDelta = new Vector2 (25,25);
				GazeText.text = "";
			}
		} 

    }
}
