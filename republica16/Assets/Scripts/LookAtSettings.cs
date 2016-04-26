using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LookAtSettings : MonoBehaviour {

	RectTransform Gaze;
	int lastID = -1;
	int curID = -1;
	public GameObject[] idObjects;

	void Start(){
		Gaze = GameObject.Find("Gaze").GetComponent<RectTransform>();
	}

    void Update() {

		if (Input.GetMouseButtonDown (0) && curID != -1) {
			PlayerPrefs.SetInt("DeviceID",curID);
			SceneManager.LoadScene("mainscene");
		} 

        //debug
       	Vector3 forward = transform.TransformDirection(Vector3.forward) * 20;
        //Debug.DrawRay(transform.position, forward, Color.green);

        RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit, 20)) {
			
			print ("hit: " + hit.collider.name);

			if (hit.collider.tag == "ID") {
				print ("id found!");

				if (hit.collider.name == "ID0") curID = 0;
				else if (hit.collider.name == "ID1") curID = 1;
				else if (hit.collider.name == "ID2") curID = 2;
				else if (hit.collider.name == "ID3") curID = 3;

				if (lastID != curID){
					lastID = curID;
					updateHighlight(curID);
				}

				Gaze.sizeDelta = new Vector2 (15, 15);
			} 
		} else {
			//hit.collider.GetComponent<Item> ().pickedByPlayerID = -1;
			Gaze.sizeDelta = new Vector2 (25,25);
		}

    }

    void updateHighlight(int id){
		idObjects[0].GetComponent<Text>().color = new Color(1,1,1,.25f);
		idObjects[1].GetComponent<Text>().color = new Color(1,1,1,.25f);
		idObjects[2].GetComponent<Text>().color = new Color(1,1,1,.25f);
		idObjects[3].GetComponent<Text>().color = new Color(1,1,1,.25f);

		idObjects[id].GetComponent<Text>().color = new Color(1,1,1,1f);
    }
}
