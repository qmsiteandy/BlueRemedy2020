using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Base : MonoBehaviour
{

    [Header("基本參數")]
    public int attackWaterCost = 1;
    protected Transform playerTrans;
    protected Animator animator;
    protected PlayerEnergy playerEnergy;
    protected PlayerControl playerControl;
    protected PlayerChange playerChange;
    protected SpriteRenderer spriteRenderer;
    protected CameraControl cameraControl;
    protected Collider2D playerCollider;

    [Header("AttackTrigger")]
    protected CircleCollider2D attackTrigger;
    protected ContactFilter2D enemyFilter;

    [Header("Input")]
    protected float skillInputDelay = 0.3f;
    protected float elapsed = 0f;
    protected int inputCount = 0;

    [Header("判斷")]
    protected int attackStep = 1;
    protected bool attacking = false;
    protected bool duringSteps = false;

    [Header("變身")]
    public GameObject changeNextFX;
    public float changeNextFX_delay;
    public GameObject changePreviousFX;
    public float changePreviousFX_delay;


    protected void BaseStart()
    {
        playerTrans = this.transform.parent.GetComponent<Transform>();
        animator = GetComponent<Animator>();
        playerEnergy = GetComponentInParent<PlayerEnergy>();
        playerControl = GetComponentInParent<PlayerControl>();
        playerChange = GetComponentInParent<PlayerChange>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraControl = playerControl.cameraControl;
        playerCollider = GetComponentInParent<Collider2D>();
        attackTrigger = this.transform.GetChild(0).GetComponent<CircleCollider2D>();
        enemyFilter.SetLayerMask(LayerMask.GetMask("Enemy"));
        enemyFilter.useTriggers = true;
    }

    protected void BaseUpdate()
    {
        if (Input.GetButtonDown("Attack")) inputCount += 1;

        if (inputCount >= attackStep)
        {
            if (PlayerControl.jumpable && !duringSteps) NormalAttack();
        }

        if (attacking == true)
        {
            elapsed += Time.deltaTime;
            if (elapsed > skillInputDelay && !duringSteps) BackNormal();
        }
    }

    protected void NormalAttack()
    {
        attacking = true;
        PlayerControl.canMove = false;

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
            default:
                if (!duringSteps) BackNormal();
                break;
        }
    }

    //從animation event呼叫攻擊扣血
    public void Damage()
    {
        playerEnergy.ModifyWaterEnergy(-attackWaterCost);

        Collider2D[] enemyColList = new Collider2D[5];
        int enemyNum = attackTrigger.OverlapCollider(enemyFilter, enemyColList);

        if (enemyNum < 1) return;

        for (int i = 0; i < enemyNum; i++)
        {
            enemyColList[i].GetComponent<Enemy_base>().TakeDamage(1);
        }
    }

    public void BackNormal()
    {
        duringSteps = false;
        attacking = false;
        attackStep = 1;
        inputCount = 0;

        animator.SetTrigger("back_idle");
        PlayerControl.canMove = true;
    }

    protected void ThisStepFinish()
    {
        duringSteps = false;
    }

    public void SetCameraTarget(Transform target, float lerpTime)
    {
        cameraControl.SetTarget(target, lerpTime);
    }

    //===============變身相關===============

    //由PlayerChange呼叫
    public void ChangeStart(bool isChangeNext)
    {
        if (isChangeNext)
        {
            animator.SetTrigger("change_next");
            if (changeNextFX != null) StartCoroutine(ChangeFX_Delay(changeNextFX, changeNextFX_delay));
        }
        else
        {
            animator.SetTrigger("change_previous");
            if (changePreviousFX != null) StartCoroutine(ChangeFX_Delay(changePreviousFX, changePreviousFX_delay));
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
        elapsed = 0f;
        inputCount = 0;

        attackStep = 1;
        attacking = false;
        duringSteps = false;

        PlayerControl.canMove = true;
    }
    IEnumerator ChangeFX_Delay(GameObject FX, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject particle = Instantiate(FX, playerTrans.position, Quaternion.identity);
        Destroy(particle, 3f);
    }
}
