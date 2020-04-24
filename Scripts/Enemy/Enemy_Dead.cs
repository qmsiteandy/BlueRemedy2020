using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dead : MonoBehaviour {

    protected Animator animator;
    public GameObject waterdrop;
    protected GameObject enemy;
    public bool isDead;

    [Header("Awake Settings")]
    protected float bornTime = 10000f;

    [Header("Health Settings")]
    public int health;
    public int healthMax = 10;

    // Use this for initialization
    protected void Awake() {
        enemy = transform.GetChild(0).gameObject;
        animator = GetComponent<Animator>();
        health = healthMax;
    }

    public void Update()
    {

    }

    public virtual void Dead()
    {
        GameObject water = Instantiate(waterdrop, enemy.transform.position, Quaternion.identity);
        water.GetComponent<WaterDrop>().enemy_dead = this;
        water.transform.SetParent(this.transform);
        water.transform.localScale = new Vector3(enemy.transform.localScale.x, 1f, 1f);
        enemy.SetActive(false);
    }

    public void NewBaby()
    {
        StartCoroutine(RebornAfterTime(bornTime));
    }

    public virtual IEnumerator RebornAfterTime(float time) {

        yield return new WaitForSeconds(time);

        isDead = false;
        enemy.SetActive(true);
        health = healthMax;
        animator.SetTrigger("Born");
    }

}
