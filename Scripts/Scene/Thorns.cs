using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : MonoBehaviour {

    private float thronAttackDelay = 0.4f;
    private float timer = 0f;
    private int thronAttack = 2;
    private PlayerControl playerControl;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerControl = collider.GetComponent<PlayerControl>();

            Attack();
        }
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                Attack();

                timer = thronAttackDelay;
            }
        }
    }

    void Attack()
    {
        playerControl.TakeDamage(thronAttack);

        if (PlayerControl.facingRight) { playerControl.rb2d.velocity = Vector3.zero; playerControl.rb2d.velocity = new Vector2(-10f, 10f); }
        else { playerControl.rb2d.velocity = Vector3.zero; playerControl.rb2d.velocity = new Vector2(10f, 10f); }
    }
}
