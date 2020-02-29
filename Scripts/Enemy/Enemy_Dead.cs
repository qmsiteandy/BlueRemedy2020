using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dead : MonoBehaviour {

    Animator animator;
    public GameObject waterdrop;
    public GameObject enemy;
    public bool isDead;
    private Enemy_base enemy_base;

    [Header("Awake Settings")]
    private float bornTime = 8f;

    [Header("Health Settings")]
    public int health;
    public int healthMax = 10;

    // Use this for initialization
    void Start () {
        enemy = transform.GetChild(0).gameObject;
        enemy_base = transform.GetComponent<Enemy_base>();
        animator = GetComponent<Animator>();
        health = healthMax;
    }

    public void Update()
    {

    }

    public void Dead()
    {

        Debug.Log(enemy.name + "die");
        GameObject water = Instantiate(waterdrop, enemy.transform.position, Quaternion.identity);
        water.GetComponent<WaterDrop>().enemy_dead = this;
        water.transform.SetParent(this.transform);
        enemy.SetActive(false);
    }

    public void NewBaby()
    {
        StartCoroutine(RebornAfterTime(bornTime));
    }

    IEnumerator RebornAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        isDead = false;
        enemy_base.BodyColliderOpen();
        enemy.SetActive(true);
        health = healthMax;
        animator.SetTrigger("Born");
        Debug.Log("born");
    }
}
