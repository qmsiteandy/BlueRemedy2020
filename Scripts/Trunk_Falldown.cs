using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk_Falldown : MonoBehaviour {

    private GameObject trunk_all;
    private GameObject trunk_trap;
    private Rigidbody2D rb2d;
    private BoxCollider2D collider_trap;
    private bool falled = false;


    // Use this for initialization
    void Start () {
        trunk_all = transform.GetChild(0).gameObject;
        trunk_trap = transform.GetChild(1).gameObject;
        rb2d = transform.GetComponentInChildren<Rigidbody2D>();
        collider_trap = GetComponent<BoxCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            trunk_all.SetActive(false);
            trunk_trap.SetActive(true);
            //float horizontal = Input.GetAxis("Horizontal");
            //rb2d.AddForceAtPosition(new Vector2(horizontal, 0), new Vector2(-1f, 1f), ForceMode2D.Force);
            trunk_trap.transform.Rotate(new Vector3(0, 50, 45)*1);
           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {

            collider_trap.enabled = false;
        }

    }


}
