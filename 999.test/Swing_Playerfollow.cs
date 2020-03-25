using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing_Playerfollow : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(this.transform);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(null);
            DontDestroyOnLoad(collision.gameObject);
        }
    }
}
