using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrial : MonoBehaviour {

    SpriteRenderer spriteRanderer;


	void Start () {

        spriteRanderer = GetComponent<SpriteRenderer>();

    }
	
	public void BeAttacked()
    {
        
    }

    IEnumerator RedFlash()
    {
        spriteRanderer.color = Color.red;
        yield return (0);
        spriteRanderer.color = Color.white;

    }
}
