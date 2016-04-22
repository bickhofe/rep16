using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {
    public float waveSpeed;
    public float offset;
    float yPos;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        yPos = Mathf.Sin(Time.time*0.5f) * waveSpeed;
        transform.position = new Vector3(0, offset + yPos, 0);
	}
}
