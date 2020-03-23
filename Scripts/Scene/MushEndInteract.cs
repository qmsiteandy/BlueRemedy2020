using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushEndInteract : MonoBehaviour {

    private Rigidbody2D rb2d;
    public Vector3 OriPos;

    public MushrInteract mushrInteract;

    void Start ()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        OriPos = this.transform.position;

        mushrInteract = this.transform.parent.GetComponent<MushrInteract>();
    }
	
	void Update ()
    {
        //if (mushrInteract.OkaStepOn) rb2d.gravityScale = 0f;
        //else rb2d.gravityScale = this.transform.position.y - OriPos.y;
        rb2d.gravityScale = (this.transform.position.y - OriPos.y) * 1.3f;

        this.transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, OriPos.y - 2f, OriPos.y + 2f));
	}

    public void AddYForce(float yForce)
    {
        Debug.Log(transform.name + " yForce: " + yForce);
        rb2d.AddForce(-Vector2.up * yForce);
    }
}
