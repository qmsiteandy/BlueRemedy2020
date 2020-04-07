using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetInit : MonoBehaviour {

    // Use this for initialization
    void Awake ()
    {
        GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectWithTag("Player").transform;
	}
}
