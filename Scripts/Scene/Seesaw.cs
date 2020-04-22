using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seesaw : MonoBehaviour {

    private bool isOpen = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Seesaw")
        {
            if(isOpen == false)
            {
                //other.GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(-0.20393f, 2.3652f);
                other.GetComponent<HingeJoint2D>().enabled = true;
                isOpen = true;
            }
        }
    }
}
