using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSkill : MonoBehaviour {

    public float throwSpeed = 15f;
    public PlayerControl playerControl;
    public FruitManager fruitManager;
    public Transform throwPoint;
    public GameObject poisonApple;
    
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Attack"))
        {
            GameObject newPoiApple = Instantiate(poisonApple, throwPoint.position, throwPoint.transform.rotation);

            Rigidbody2D rigid = newPoiApple.GetComponent<Rigidbody2D>();
            if (playerControl.facingRight) rigid.velocity = throwSpeed * throwPoint.transform.right;
            else rigid.velocity = -1 * throwSpeed * throwPoint.transform.right;

            fruitManager.FirstLoseFress(FruitmanData.InfoList[1].attack_fressLoss);
        }

        if (Input.GetButtonDown("Defence"))
        {

        }

        if (Input.GetButtonDown("Heal"))
        {

        }

    }
}
