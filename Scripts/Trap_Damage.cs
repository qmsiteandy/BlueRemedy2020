using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Damage : MonoBehaviour {

    public GameObject attackTarget = null;
    public int damage = 200;
    private Rigidbody2D rb2d;

    private bool falled = false;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !falled)
        {
            attackTarget = collision.gameObject;
            Damage();

            falled = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            attackTarget = null;
        }
    }

    public void Damage()
    {
        attackTarget.GetComponent<PlayerControl>().TakeDamage(damage);
    }
}
