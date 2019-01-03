using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Grass : MonoBehaviour {

    PlayerControl playerControl;
    float spawnGrassTime;
    GrassTest grassTest;

    // Use this for initialization
    void Start () {
        playerControl = GetComponent<PlayerControl>();
        grassTest = GetComponent<GrassTest>();
    }
	
	// Update is called once per frame
	void Update () {


        if (playerControl.grounded && Mathf.Abs(playerControl.xSpeed) > 0)
        {
            spawnGrassTime += Time.deltaTime; //偵測每一針時間
            if (spawnGrassTime > 0.18f)
            {
                spawnGrassTime = 0.0f;
                grassTest.SpawnGrass();
            }
        }
    }
}
