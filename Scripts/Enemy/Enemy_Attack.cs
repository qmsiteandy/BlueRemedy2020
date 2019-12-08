using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{

    public int attackDamage = 15;
    public float attackDelay = 2f;
    private Enemy_base enemy_base;
    Animator animator;

    public float elapsed = 0f;
    public bool canAttack = false;
    public GameObject attackTarget = null;

    public Rigidbody2D rb2d;
    public float g = 9.81f;  //g = 9.81 m/s^2
    public float speed = 0.8f; //物體transform在拋物線路徑上移動的速度，此速度不影響拋物線的形狀
    public float V0 = 8.0f; //初速度

    // Use this for initialization
    void Awake()
    {
        animator = transform.GetComponentInParent<Animator>();
    }
    void Start()
    {
        enemy_base = transform.GetComponentInParent<Enemy_base>();
        rb2d = transform.GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (elapsed < attackDelay && !canAttack)
        {
            elapsed += Time.deltaTime;
        }
        else if (elapsed > attackDelay)
        {
            canAttack = true;
            elapsed = 0f;
        }

        Attack();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            attackTarget = other.gameObject;
            enemy_base.isAttacking = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            attackTarget = null;
            enemy_base.isAttacking = false;
        }
    }

    public void Attack()
    {
        if (canAttack && attackTarget != null)
        {
            animator.SetTrigger("Attack");

            canAttack = false;
        }
    }

    public void Damage()
    {
        if (attackTarget == null) return;
        attackTarget.GetComponent<PlayerControl>().TakeDamage(attackDamage);
    }




}
