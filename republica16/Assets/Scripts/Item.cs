using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    public Main MainScript;

    public int itemID;
    public int zoneID;
    public Vector3 itemPos;
    public int curIsland;

    Rigidbody rb;

    // Use this for initialization
    void Start() {
        MainScript = GameObject.Find("Main").GetComponent<Main>();
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
       // var rnd = Random.Range(-.01f, .01f);
        //itemPos = transform.position + new Vector3(rnd, 0, rnd);

        //itemPos = transform.position;
        //transform.position = itemPos;
    }

    void OnTriggerEnter(Collider Portal) {
        print("Enter portal");

        // ID der Insel des Colliders finden
        int IslandID = Portal.transform.parent.GetComponent<Island>().IslandID;

        if (IslandID == 0) curIsland = 3;
        else if (IslandID == 1) curIsland = 2;
        else if (IslandID == 2) curIsland = 0;
        else if (IslandID == 3) curIsland = 1;

        TeleportItem(MainScript.startPoint[curIsland]);
    }

    public void UpdateItem() {
        transform.position = itemPos;
    }

    public void TeleportItem(Vector3 teleportPos) {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = teleportPos;
        transform.eulerAngles = Vector3.zero;
    }

    public void ResetItem(Vector3 startPos) {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPos;
        transform.eulerAngles = Vector3.zero;
    }
}
