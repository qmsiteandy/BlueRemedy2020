using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Attack_fish : MonoBehaviour {
    public int attackDamage = 15;
    public float attackDelay = 1.5f;
    private Enemy_base_Fish enemy_base_fish;
    Animator animator;
    //public CameraControl cameraControl;

    public float elapsed = 0f;
    public bool canAttack = false;
    public GameObject attackTarget = null;
    public bool Attack_Wait = false;

    // Use this for initialization
    void Awake()
    {
        animator = transform.GetComponentInParent<Animator>();
    }
    void Start()
    {
        enemy_base_fish = transform.GetComponentInParent<Enemy_base_Fish>();
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
            Attack_Wait = false;
        }

        Attack();

        if (attackTarget == null)
        {
            Attack_Wait = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            attackTarget = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            attackTarget = null;
        }
    }

    public void Attack()
    {
        if (canAttack && attackTarget != null && enemy_base_fish.isFreeze == false)
        {
            animator.SetTrigger("Attack");

            canAttack = false;
        }
    }

    public void Damage()
    {

        if (attackTarget != null)
        {
            attackTarget.GetComponent<PlayerControl>().TakeDamage(attackDamage);
        }
    }
}
