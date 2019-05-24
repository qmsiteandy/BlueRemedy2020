using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrial : MonoBehaviour {

    SpriteRenderer spriteRanderer;

    CircleCollider2D enemyCol;
    ContactFilter2D playerFilter;

    bool isAttacking = false;


    void Start () {

        spriteRanderer = GetComponent<SpriteRenderer>();

        enemyCol = GetComponent<CircleCollider2D>();
        playerFilter.SetLayerMask(LayerMask.GetMask("Player"));

    }

    

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player") collider.transform.parent.GetComponent<PlayerEnergy>().ModifyDirt(2);
    }
	
	public void TakeDamage(int damage)
    {
        StartCoroutine(RedFlash());
    }

    IEnumerator RedFlash()
    {
        spriteRanderer.color = Color.red;
        yield return (0);
        spriteRanderer.color = Color.white;

    }
}
