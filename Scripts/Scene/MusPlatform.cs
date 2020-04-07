using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusPlatform : MonoBehaviour {

    private GameObject mus_platform;
    
    // Use this for initialization
	void Start () {
        mus_platform = transform.GetChild(1).gameObject;
        mus_platform.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlatformOpen()
    {
        mus_platform.SetActive(true);
    }
}
