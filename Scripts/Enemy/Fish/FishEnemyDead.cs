using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEnemyDead : Enemy_Dead {

    private GameObject water;
    private GameObject dropPlace;
    private FishEnemyControl fishenemycontrol;

    // Use this for initialization
    void Start() {
        dropPlace = transform.GetChild(2).gameObject;
        fishenemycontrol = transform.GetComponent<FishEnemyControl>();
    }

    // Update is called once per frame\

    void Update()
    {
        DeadUpdate();
    }

    public override void Dead()
    {
        water = Instantiate(waterdrop, dropPlace.transform.position, Quaternion.identity);
        water.GetComponent<WaterDrop>().enemy_dead = this;
        water.transform.SetParent(this.transform);
        water.transform.localScale = new Vector3(enemy.transform.localScale.x, 1f, 1f);
        enemy.SetActive(false);
    }

    public override IEnumerator RebornAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        isDead = false;
        fishenemycontrol.BodyColliderOpen();
        animator.SetTrigger("Born");
        enemy.SetActive(true);
        health = healthMax;
        
    }
}
