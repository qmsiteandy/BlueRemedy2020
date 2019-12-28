using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordPointManager : MonoBehaviour {

    private static Vector3 playerRecordPos;
    private static GameObject player;

	void Start ()
    {
        player = GameObject.Find("Player");
        playerRecordPos = player.transform.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerRecordPos = this.transform.position;
            Debug.Log(playerRecordPos);
        }
    }

    public static Vector3 Get_playerRecordPos()
    {
        return playerRecordPos;
    }
}
