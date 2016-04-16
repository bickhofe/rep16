using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LookAt : MonoBehaviour {

	RectTransform Gaze;
	Text GazeText;

    public GameObject VRCamHead;
    // Update is called once per frame

	void Start(){
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
				GazeText.text = hit.collider.name+": "+hit.collider.GetComponent<Item> ().itemID;
			} else {
				Gaze.sizeDelta = new Vector2 (25,25);
				GazeText.text = "";
			}
		} 

    }
}
