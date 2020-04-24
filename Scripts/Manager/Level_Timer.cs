using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Timer : MonoBehaviour {

    private float timer = 0f;
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
    }

    public float Get_LevelTime()
    {
        return (timer);
    }
}
