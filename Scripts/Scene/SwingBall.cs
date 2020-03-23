using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingBall : MonoBehaviour {

    private Rigidbody2D rb2d;
    public float leftPushRange;
    public float rightPushRange;
    public float vecThreshold;
    
    // Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.angularVelocity = vecThreshold;
	}
	
	// Update is called once per frame
	void Update () {
        Push();
	}

    public void Push()
    {
        if(transform.rotation.z > 0 
            && transform.rotation.z < rightPushRange
            && (rb2d.angularVelocity > 0)
            && rb2d.angularVelocity < vecThreshold)
        {
            rb2d.angularVelocity = vecThreshold;
        }
        else if(transform.rotation.z < 0
            && transform.rotation.z > leftPushRange
            && (rb2d.angularVelocity < 0)
            && rb2d.angularVelocity > vecThreshold * -1)
        {
            rb2d.angularVelocity = vecThreshold * -1;
        }
    }
}
