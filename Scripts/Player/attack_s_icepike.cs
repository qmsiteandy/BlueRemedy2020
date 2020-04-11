using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_s_icepike : MonoBehaviour {

    [Header("AttackTrigger")]
    protected CircleCollider2D attackTrigger;
    protected ContactFilter2D enemyFilter;

    // Use this for initialization
    void Start()
    {
        attackTrigger = this.GetComponent<CircleCollider2D>();
        enemyFilter.SetLayerMask(LayerMask.GetMask("Enemy"));
        enemyFilter.useTriggers = true;
    }

    void Damage()
    {
        Collider2D[] enemyColList = new Collider2D[5];
        int enemyNum = attackTrigger.OverlapCollider(enemyFilter, enemyColList);

        if (enemyNum > 0)
        {
            for (int i = 0; i < enemyNum; i++)
            {
                if (enemyColList[i].GetComponent<Enemy_Dead>().isDead == true) break;

                Enemy_base enemy_Base = enemyColList[i].GetComponent<Enemy_base>();
                enemy_Base.TakeDamage(1);
                enemy_Base.KnockBack(PlayerControl.facingRight ? Vector3.right : Vector3.left, 150f);
            }
        }
    }

    void Finish()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
