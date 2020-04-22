using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanAtkObj : MonoBehaviour {

    Animator animator;
    public bool isDead = false;
    public int health;
    public int healthMax = 2;
    GameObject ThisObj;
    SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start () {
        animator = transform.GetComponentInParent<Animator>();
        health = healthMax;
        ThisObj = gameObject.transform.parent.gameObject;
        spriteRenderer = transform.GetComponentInParent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                
                animator.SetTrigger("Dead");

                isDead = true;
            }
            StartCoroutine(ChangeColor(new Color(1f, 1f, 1f,0.5f), 0.1f));
        }
    }

    public void Dead()
    {
        Destroy(ThisObj);
    }

    IEnumerator ChangeColor(Color color, float colorChangeTime)
    {
        spriteRenderer.color = color;

        yield return new WaitForSeconds(colorChangeTime);

        spriteRenderer.color = new Color(1, 1, 1);
    }
}
