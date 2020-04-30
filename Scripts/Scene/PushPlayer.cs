using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPlayer : MonoBehaviour {

    private Rigidbody2D PlayerRb2d;
    public float thrust = 5;

    // Use this for initialization
    void Start () {
        PlayerRb2d = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        PlayerRb2d.AddForce(new Vector2(-10,0)*thrust);
    //    }
    //}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerRb2d.AddForce(new Vector2(-90, -20) * thrust);
        }
    }
}
