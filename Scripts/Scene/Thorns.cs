using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : MonoBehaviour {

    private float thronAttackDelay = 0.4f;
    private float timer = 0f;
    private int thronAttack = 2;
    private PlayerControl playerControl;

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                Attack(collider.gameObject);
                timer = thronAttackDelay;
            }
        }
    }

    void Attack(GameObject Oka)
    {
        playerControl = Oka.GetComponent<PlayerControl>();
        playerControl.TakeDamage(thronAttack);

        if (PlayerControl.facingRight) { playerControl.rb2d.velocity = Vector3.zero; playerControl.rb2d.AddForce(new Vector2(-playerControl.jumpForce * 0.2f, playerControl.jumpForce * 0.3f)); }
        else { playerControl.rb2d.velocity = Vector3.zero; playerControl.rb2d.AddForce(new Vector2(playerControl.jumpForce * 0.2f, playerControl.jumpForce * 0.3f)); }
    }

}
