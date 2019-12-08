using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour {

    GameObject target;
    public GameObject waterdrop;
    public Enemy_base enemy_base;

    public int waterEnergyCharge = 30;
 

    // Use this for initialization
    void Start () {
    
    }
	
	// Update is called once per frame
	void Update () {
     
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            target = collision.gameObject;

            enemy_base.NewBaby();

            collision.GetComponentInParent<PlayerEnergy>().ModifyWaterEnergy(waterEnergyCharge);

            Destroy(this.gameObject);
        }
    }
    

    

}
