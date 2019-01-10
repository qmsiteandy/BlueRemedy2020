using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatermelonSkill : MonoBehaviour {

    public PlayerControl playerControl;
    public FruitManager fruitManager;
    

    void Start () {

        /*initial before SetActive(false)*/

        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Attack"))
        {
            fruitManager.FirstLoseFresh(FruitmanData.InfoList[2].attack_freshLoss);
        }


        if (Input.GetButtonDown("Defence"))
        {
            fruitManager.FirstLoseFresh(FruitmanData.InfoList[2].defence_freshLoss);
        }
        //if (isDefensing) ShieldState();


        if (Input.GetButtonDown("Heal"))
        {
            FruitmanBase firstFruitBase = fruitManager.firstOne.GetComponent<FruitmanBase>();

            int healValue = (int)(FruitmanData.InfoList[2].heal * firstFruitBase.fresh / firstFruitBase.freshMax);
            playerControl.Heal(healValue);

            fruitManager.FirstLoseFresh(10000);
        }
    }
}
