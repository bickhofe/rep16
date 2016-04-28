using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BTController : MonoBehaviour {

	public Text debugtext;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		debugtext.text = "Fire1: "+Input.GetButton("Fire1")+"\n"+"Fire2: "+Input.GetButton("Fire2")+"\n"+"Fire3: "+Input.GetButton("Fire3")+"\n"+"Hori: "+Input.GetAxis("Horizontal")+"\n"+"Verti: "+Input.GetAxis("Vertical");
	
	}
}
