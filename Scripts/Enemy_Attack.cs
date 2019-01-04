using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour {

    public int attackDamage = 200;
    public float attackDelay = 1.5f;
    private Enemy_base enemy_base;
    Animator animator;

    public float elapsed = 0f;
    public bool canAttack = false;
    public GameObject attackTarget = null;
    // Use this for initialization
    void Awake()
    {
        animator = transform.GetComponentInParent<Animator>();
    }
    void Start () {
        enemy_base = transform.GetComponentInParent<Enemy_base>();
    }
	
	// Update is called once per frame
	void Update () {

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
            animator.SetTrigger("attack");

            canAttack = false;
        }
    }

    public void Damage()
    {
        if (attackTarget == null) return;
        attackTarget.GetComponent<PlayerControl>().TakeDamage(attackDamage);
    }

    

}
