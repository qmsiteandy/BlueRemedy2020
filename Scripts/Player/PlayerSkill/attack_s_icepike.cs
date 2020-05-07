using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_s_icepike : MonoBehaviour {

    [Header("AttackTrigger")]
    protected CircleCollider2D attackTrigger;
    protected ContactFilter2D enemyFilter;
    protected ContactFilter2D canAtkObjFilter;

    // Use this for initialization
    void Start()
    {
        attackTrigger = this.GetComponent<CircleCollider2D>();
        enemyFilter.SetLayerMask(LayerMask.GetMask("Enemy"));
        canAtkObjFilter.SetLayerMask(LayerMask.GetMask("CanAtkObj"));
    }

    void Damage()
    {
        Collider2D[] enemyColList = new Collider2D[5];
        int enemyCount = attackTrigger.OverlapCollider(enemyFilter, enemyColList);

        if (enemyCount > 0)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                if (enemyColList[i].GetComponent<Enemy_Dead>().isDead == true) continue;

                Enemy_base enemy_Base = enemyColList[i].GetComponent<Enemy_base>();
                enemy_Base.TakeDamage(1);
                enemy_Base.KnockBack(PlayerControl.facingRight ? Vector3.right : Vector3.left, 100f);
            }
        }

        Collider2D[] atkObjColList = new Collider2D[5];
        int ObjCount = attackTrigger.OverlapCollider(canAtkObjFilter, atkObjColList);

        if (ObjCount > 0)
        {
            for (int i = 0; i < ObjCount; i++)
            {
                CanAtkObj canAtkObj = atkObjColList[i].GetComponent<CanAtkObj>();
                if (canAtkObj != null) canAtkObj.TakeDamage(1);
            }
        }
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        Enemy_base enemy_Base = collision.gameObject.GetComponent<Enemy_base>();
    //        enemy_Base.TakeDamage(1);
    //        enemy_Base.KnockBack(PlayerControl.facingRight ? Vector3.right : Vector3.left, 150f);
    //    }
    //}

    void Finish()
    {
        Destroy(this.transform.parent.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("WaterArea"))
        {
            GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Scene"; GetComponentInChildren<SpriteRenderer>().sortingOrder = -1;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("WaterArea"))
        {
            GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Skill"; GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
        }
    }
}
