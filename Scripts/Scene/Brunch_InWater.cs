using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brunch_InWater : MonoBehaviour {

    private Water_Area water_area;
    private int WaterLayerID = 14;
    private Rigidbody2D rb2d;
    public float FloatForce = 30f;

    // Use this for initialization
    void Start () {
        rb2d = transform.GetComponent<Rigidbody2D>();
        rb2d.drag = 3f;
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == WaterLayerID)
        {
            water_area = collision.GetComponent<Water_Area>();
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            
            if (water_area.waveCrest - transform.position.y > 0.4f * GetComponent<BoxCollider2D>().size.y) rb2d.AddForce(Vector2.up * FloatForce);
            else if (water_area.waveCrest - transform.position.y > 0f) rb2d.AddForce(Vector2.up * FloatForce * (water_area.waveCrest - transform.position.y / 1f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            //rbFreezeAll(0.2f);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            if (collision.transform.position.y - transform.position.y > 0)
            {
                rb2d.AddForce(Vector2.up * 40f);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    IEnumerator rbFreezeAll(float Freezetime)
    {
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(Freezetime);
        rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

}
