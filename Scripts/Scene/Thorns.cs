using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : MonoBehaviour {

    private float attackCycle = 0.4f;
    private int thronAttack = 2;
    private PlayerControl playerControl;
    private Rigidbody2D playerRb2d;
    private Coroutine attack_Routine;


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerControl = collider.GetComponent<PlayerControl>();
            playerRb2d = collider.GetComponent<Rigidbody2D>();

            if (attack_Routine != null) StopCoroutine(attack_Routine);
            attack_Routine = StartCoroutine(AttackCycle());
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (attack_Routine != null) { StopCoroutine(attack_Routine); attack_Routine = null; }
        }
    }

    IEnumerator AttackCycle()
    {
        float timer = 0f;

        while (true)
        {
            if(timer <= 0f)
            {
                {
                    playerControl.TakeDamage(thronAttack);

                    Vector2 hitBackF_dir = new Vector2(-playerRb2d.velocity.x, -playerRb2d.velocity.y).normalized;
                    playerRb2d.velocity = hitBackF_dir * 10f;
                }

                timer = attackCycle;
            }
            else
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }

    }
}
