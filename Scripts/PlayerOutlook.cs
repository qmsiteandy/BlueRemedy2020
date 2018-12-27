using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOutlook : MonoBehaviour {

    public float FX_deleteTime = 1f;
    public GameObject changeFX_Prefab;
    public Sprite[] playerSprite;
    
    private SpriteRenderer spriteRender;


	void Start () {
        spriteRender = GetComponent<SpriteRenderer>();
	}
	
    public void OutlookChange(int id)
    {
        //Debug.Log("OutlookChange ID= " + id);
        StartCoroutine(ChangeFX());
        spriteRender.sprite = playerSprite[id];
    }

    IEnumerator ChangeFX()
    { 
        GameObject FX_Object = (Instantiate(changeFX_Prefab, transform.position, Quaternion.identity));

        yield return new WaitForSeconds(FX_deleteTime);

        Destroy(FX_Object);
    }
}
