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

    void Update()
    {
        Collider2D[] playerCol= { null };
        enemyCol.OverlapCollider(playerFilter, playerCol);

        if (playerCol[0] != null && !isAttacking) { playerCol[0].transform.parent.GetComponent<PlayerEnergy>().ModifyDirt(+2); isAttacking = true; }
        else if (playerCol[0] == null) isAttacking = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Player") collision.transform.parent.GetComponent<PlayerEnergy>().ModifyDirt(-2);
    }
	
	public void BeAttacked()
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
