using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : MonoBehaviour {

    public float thronAttackTimer = 0f;
    private float thronAttackDelay = 0.8f;
    private int thronAttack = 2;
    private PlayerControl playerControl;

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerControl = collider.gameObject.GetComponent<PlayerControl>();

            thronAttackTimer -= Time.deltaTime;
            if (thronAttackTimer < 0f)
            {
                playerControl.TakeDamage(thronAttack); thronAttackTimer = thronAttackDelay;

                if (playerControl.facingRight) { playerControl.rb2d.velocity = Vector3.zero; playerControl.rb2d.AddForce(new Vector2(-playerControl.jumpForce * 1.5f, playerControl.jumpForce * 0.6f)); }
                else { playerControl.rb2d.velocity = Vector3.zero; playerControl.rb2d.AddForce(new Vector2(playerControl.jumpForce * 1.5f, playerControl.jumpForce * 0.6f)); }
            }
        }
    }

}
