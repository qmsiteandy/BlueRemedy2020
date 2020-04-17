using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class special_l_waterCanon : MonoBehaviour {

    public int damageAmount = 1;
    private Collider2D attackTrigger;
    private ContactFilter2D enemyFilter;
    private float canonAngle = 0f;

    // Use this for initialization
    void Start ()
    {
        attackTrigger = this.GetComponent<Collider2D>();

        enemyFilter.SetLayerMask(LayerMask.GetMask("Enemy"));
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
                enemy_Base.TakeDamage(damageAmount);
                enemy_Base.KnockBack(new Vector2(Mathf.Cos(canonAngle), Mathf.Sin(canonAngle)), 150f);
            }
        }
    }

    public void SetAngle(float angle)
    {
        canonAngle = angle;
        canonAngle = canonAngle * Mathf.PI / 180f;
    }

    void Finish()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
