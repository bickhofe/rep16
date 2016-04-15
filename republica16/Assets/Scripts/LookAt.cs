using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {

    public GameObject VRCamHead;
    // Update is called once per frame

    void Update() {
        //debug
        Vector3 forward = VRCamHead.transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(VRCamHead.transform.position, forward, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(VRCamHead.transform.position, VRCamHead.transform.forward, out hit)) {

            print("hit: " + hit.collider.name);

            if (hit.collider.name == "Cube") {
                print("Cube found!");
            }
        }

    }
}
