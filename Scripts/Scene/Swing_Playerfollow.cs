using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing_Playerfollow : MonoBehaviour {

    private PlayerControl playerControl;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerControl = collision.gameObject.GetComponent<PlayerControl>();

            if(playerControl.objUnderFoot == this.gameObject)
            {
                SetPlayerAsChild(collision.transform, true);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.rotation = Quaternion.Euler(Vector3.zero);

            if (playerControl != null && playerControl.objUnderFoot != this.gameObject)
            {
                SetPlayerAsChild(collision.transform, false);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SetPlayerAsChild(collision.transform, false);
        }
    }

    void SetPlayerAsChild(Transform playerTrans, bool truefalse)
    {
        if (truefalse)
        {
            playerTrans.SetParent(this.transform);
        }
        else
        {
            playerTrans.SetParent(null);
        }
    }
}
