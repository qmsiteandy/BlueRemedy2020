using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform target;
    public float smoothSpeed = 0.05f;
    public float lookUpMove = 1.5f;
    public float lookDownMove = 4f;

    private Vector3 offset;
    private Vector3 AimPos;
    bool isFollowMode = true;
    
	// Use this for initialization
	void Start () {
        offset = transform.position - target.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (isFollowMode) { AimPos = target.position + offset; }   

        Vector3 smoothPos = Vector3.Lerp(transform.position, AimPos, smoothSpeed);
        transform.position = smoothPos;
    }


    public void PlayerLookUp()
    {
        AimPos += new Vector3(0f, lookUpMove, 0f);
        isFollowMode = false;
    }


    public void PlayerLookDown()
    {
        AimPos -= new Vector3(0f, lookDownMove, 0f);
        isFollowMode = false;
    }


    public void SetAimPos(float xAim, float yAim)
    {
        AimPos = new Vector3(xAim, yAim, 0f);
        isFollowMode = false;
    }


    public void BackNormal()
    {
        isFollowMode = true;
    }
}
