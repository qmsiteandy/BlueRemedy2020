using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyDead : Enemy_Dead {

    private GameObject water;
    private GroundEnemyControl groundEmenyControl;
    private GameObject dropPlace;

    // Use this for initialization
    void Start () {
        groundEmenyControl = transform.GetComponent<GroundEnemyControl>();
        dropPlace = transform.GetChild(2).gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
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
        groundEmenyControl.BodyColliderOpen();
        transform.position = groundEmenyControl.centerPos;
        enemy.SetActive(true);
        health = healthMax;
        animator.SetTrigger("Born");
    }
}
