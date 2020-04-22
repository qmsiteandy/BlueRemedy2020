using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{

    public int attackDamage = 15;
    public float attackDelay = 2f;
    protected Enemy_base enemy_base;
    Animator animator;
    //public CameraControl cameraControl;

    public float elapsed = 0f;
    public bool canAttack = false;
    public GameObject attackTarget = null;
    public bool Attack_Wait = false;


    // Use this for initialization
    public void Awake()
    {
        animator = transform.GetComponentInParent<Animator>();
        enemy_base = transform.GetComponentInParent<Enemy_base>();
    }

    // Update is called once per frame
     public void Update()
    {

        if (elapsed < attackDelay && !canAttack)
        {
            elapsed += Time.deltaTime;
        }
        else if (elapsed > attackDelay)
        {
            canAttack = true;
            elapsed = 0f;
            Attack_Wait = false;
        }

        Attack();

        if (attackTarget == null)
        {
            Attack_Wait = false;
        }

    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            attackTarget = other.gameObject;
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            attackTarget = null;
        }
    }

    public void Attack()
    {
        if (canAttack && attackTarget != null && enemy_base.isFreeze == false)
        {
            animator.SetTrigger("Attack");

            canAttack = false;
        }
    }

    public virtual void Damage()
    {

        if (attackTarget != null)
        {
            attackTarget.GetComponent<PlayerControl>().TakeDamage(attackDamage);
        }
        
    }

}
