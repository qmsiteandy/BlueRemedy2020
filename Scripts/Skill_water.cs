using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_water : MonoBehaviour {


    CircleCollider2D attackTrigger;
    public ContactFilter2D enemyFilter;
    

    public float skillInputDelay = 0.35f;
    private float elapsed = 0f;

    private int attackStep = 1;
    private bool attacking = false;
    private bool duringSteps = false;

    private PlayerControl playerControl;
    private Animator animator;

    void Start ()
    {
        playerControl = GetComponent<PlayerControl>();
        animator = GetComponent<Animator>();

        attackTrigger = this.transform.GetChild(2).GetComponent<CircleCollider2D>();
        enemyFilter.SetLayerMask(LayerMask.GetMask("Enemy"));
        enemyFilter.useTriggers = true;
    }
	
	void Update ()
    {

        if (Input.GetButtonDown("Attack") && !duringSteps)
        {
            if (playerControl.grounded == true) NormalAttackInput();
        }

        elapsed += Time.deltaTime;
        if (attacking == true)
        {
            if (elapsed > skillInputDelay) BackNormal();
        }
    }

    void NormalAttackInput()
    {
        attacking = true;
        playerControl.canMove = false;

        switch (attackStep)
        {
            case 1:
                animator.SetTrigger("attack_1"); elapsed = 0f; attackStep += 1; duringSteps = true;
                break;
            case 2:
                animator.SetTrigger("attack_2"); elapsed = 0f; attackStep += 1; duringSteps = true;
                break;
            case 3:
                animator.SetTrigger("attack_3"); elapsed = 0f; attackStep += 1; duringSteps = true;
                break;
            case 4:
                BackNormal(); 
                break;
        }
    }

    //從animation event呼叫攻擊扣血
    void NormalAttack()
    {
        Collider2D[] enemyColList = new Collider2D[5];
        int enemyNum = attackTrigger.OverlapCollider(enemyFilter, enemyColList);

        if (enemyNum < 1) return;

        for(int i = 0; i < enemyNum; i++)
        {
            enemyColList[i].transform.parent.GetComponent<Enemy_base>().TakeDamage(1);
            //enemyColList[i].GetComponent<EnemyTrial>().TakeDamage(1);
        }
    }

    void BackNormal()
    {
        attacking = false;
        attackStep = 1;
        animator.SetTrigger("back_idle");

        playerControl.canMove = true;
    }

    void ThisStepFinish()
    {
        duringSteps = false;
    }
}
