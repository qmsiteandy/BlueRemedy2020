using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_base : MonoBehaviour {

    //繼承用的父腳本，變數&函式盡量都不要是private，否則子腳本讀不到
    //繼承後protected->private，public->public

    protected GameObject enemy;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb2d;
    protected Enemy_Dead enemy_dead;
    protected Enemy_Attack enemy_attack;

    public bool isAttacking;
    public bool isFreeze = false;

    //Start & Upda
    protected void BaseAwake()
    {
        enemy = transform.GetChild(0).gameObject;
        enemy_attack = transform.GetComponentInChildren<Enemy_Attack>();
        enemy_dead = transform.GetComponent<Enemy_Dead>();

        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }


    public void Attack_Damage()
    {
        enemy_attack.Damage();
    }
    public void AttackStart()
    {
        isAttacking = true;
        enemy_attack.Attack_Wait = false;
    }
    public void AttackOver()
    {
        isAttacking = false;
        enemy_attack.Attack_Wait = true;
    }

    //TakeDamage虛擬函式，會在子腳本覆寫
    public virtual void TakeDamage(int damage) { }

    //被攻擊時擊退
    public void KnockBack(Vector3 direction, float force)
    {
        this.rb2d.AddForce(direction * force);
    }

    protected IEnumerator ChangeColor(Color color, float colorChangeTime)
    {
        spriteRenderer.color = color;

        yield return new WaitForSeconds(colorChangeTime);

        spriteRenderer.color = new Color(1, 1, 1);
    }

    protected IEnumerator Freeze(float freezeTime)
    {
        isFreeze = true;

        yield return new WaitForSeconds(freezeTime);

        isFreeze = false;
    }

    
}


