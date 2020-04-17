using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    [Header("基本參數")]
    public int attack3WaterCost = 5;
    protected Transform playerTrans;
    protected Animator animator;
    protected PlayerEnergy playerEnergy;
    protected PlayerControl playerControl;
    protected PlayerChange playerChange;
    protected SpriteRenderer spriteRenderer;
    protected Collider2D playerCollider;

    [Header("AttackTrigger")]
    protected CircleCollider2D attackTrigger;
    protected ContactFilter2D enemyFilter;

    [Header("Input")]
    protected float skillInputDelay = 0.5f;
    protected int normalattack_input = 0;
    protected Coroutine inputDelayRoutine;

    [Header("判斷")]
    protected int normalattack_step = 0;
    protected bool attacking = false;


    protected void BaseStart()
    {
        playerTrans = this.transform.parent.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        playerEnergy = GetComponentInParent<PlayerEnergy>();
        playerControl = GetComponentInParent<PlayerControl>();
        playerChange = GetComponentInParent<PlayerChange>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponentInParent<Collider2D>();
        attackTrigger = this.transform.GetChild(0).GetComponent<CircleCollider2D>();
        enemyFilter.SetLayerMask(LayerMask.GetMask("Enemy"));
    }

    protected void BaseUpdate()
    {
        if (Input.GetButtonDown("Attack") && PlayerStatus.canSkill)
        {
            if (PlayerControl.footLanding) normalattack_input += 1;
            else SkyAttack();
        }

        if (normalattack_input > normalattack_step)
        {
            if (!attacking) NormalAttack();
        }
    }

    protected void NormalAttack()
    {
        SetAttacking(true);
        normalattack_step += 1;
        
        switch (normalattack_step)
        {
            case 1:
                {
                    animator.SetTrigger("attack_1");

                    if (inputDelayRoutine != null) StopCoroutine(inputDelayRoutine);
                    inputDelayRoutine = StartCoroutine(Normalinput_Delaying());
                }
                break;
            case 2:
                {
                    animator.SetTrigger("attack_2");

                    if (inputDelayRoutine != null) StopCoroutine(inputDelayRoutine);
                    inputDelayRoutine = StartCoroutine(Normalinput_Delaying());
                }
                break;
            case 3:
                {
                    //"attack_3" animation最後要呼叫BackIdle 
                    animator.SetTrigger("attack_3");
                    playerEnergy.ModifyWaterEnergy(-attack3WaterCost);

                    if (inputDelayRoutine != null) StopCoroutine(inputDelayRoutine);
                }   
                break;
        }
    }

    protected void SkyAttack()
    {
        animator.SetTrigger("sky_attack");

        SetAttacking(true);
    }

    //從animation event呼叫攻擊扣血
    protected void Damage()
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
                enemy_Base.KnockBack(PlayerControl.facingRight? Vector3.right: Vector3.left, 100f);
            }
        }
    }

    protected void SetAttacking(bool truefalse)
    {
        attacking = truefalse;
        PlayerStatus.isSkilling = truefalse;
    }

    public void BackIdle()
    {
        //animator.SetTrigger("back_idle");

        SetAttacking(false);
        if (inputDelayRoutine != null) StopCoroutine(inputDelayRoutine);

        normalattack_step = 0;
        normalattack_input = 0; 
    }

    IEnumerator Normalinput_Delaying()
    {
        float elapsed = 0f;

        while (elapsed < skillInputDelay) { elapsed += Time.deltaTime;  yield return null; }

        BackIdle();
    }

    protected void ThisStepFinish()
    {
        attacking = false;

        if(normalattack_step >= 3) BackIdle();
    }

    #region ===============變身相關===============

    //由PlayerChange呼叫
    public void ChangeStart(bool isChangeNext)
    {
        if (isChangeNext)
        {
            animator.SetTrigger("change_next");
        }
        else
        {
            animator.SetTrigger("change_previous");
        }
    }
    //animation變身完成後呼叫PlayerChange.cs變成另一隻
    void TransformAnimFinish()
    {
        playerChange.ChangeFinish();
    }
    //變身完成這隻開啟時呼叫
    public void TransformReset()
    {
        BackIdle();
    }
    void SleepAwake()
    {
        playerControl.SleepAwake();
    }

    #endregion ===============變身相關===============
}
