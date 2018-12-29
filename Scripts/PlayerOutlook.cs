using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOutlook : MonoBehaviour {

    public float FX_deleteTime = 0.75f;
    public GameObject changeFX_Prefab;
    public Sprite[] playerSprite;
    
    private SpriteRenderer spriteRender;


	void Start () {
        spriteRender = GetComponent<SpriteRenderer>();
	}
	
    public void OutlookChange(int id)
    {
        //Debug.Log("OutlookChange ID= " + id);
        GameObject FX_Object = (Instantiate(changeFX_Prefab, transform.position, Quaternion.identity));
        
        spriteRender.sprite = playerSprite[id];

        Destroy(FX_Object, FX_deleteTime);
    }
}
