using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class PlayerOutlook : MonoBehaviour {

    public float FX_deleteTime = 0.75f;
    public GameObject changeFX_Prefab;
    public AnimatorController playerAnim;
    public AnimatorOverrideController[] fruitPlayerAnim;

    private SpriteRenderer spriteRender;
    private Animator animator;


	void Start () {
        spriteRender = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
	
    public void OutlookChange(int id)
    {
        GameObject FX_Object = (Instantiate(changeFX_Prefab, transform.position, Quaternion.identity));
        Destroy(FX_Object, FX_deleteTime);

        if (id == 0) animator.runtimeAnimatorController = playerAnim;
        else if (fruitPlayerAnim[id] == null) return;
        else animator.runtimeAnimatorController = fruitPlayerAnim[id];

        
    }
}
