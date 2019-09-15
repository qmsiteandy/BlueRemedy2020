using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePass : MonoBehaviour {

    public GameObject treeWater;
    public GameObject root_trigger;
    public Transform rightEnd, leftEnd;
    public GameObject waterSplashFX;

    private Animator treeWaterAnim;
    private GameObject Oka;
    private bool facingRight;

    // Use this for initialization
    void Start () {

        treeWaterAnim = GetComponent<Animator>();
    }
	
    //由playerController呼叫
	public void Passing(GameObject OkaObj)
    {
        //樹中水毛細上升anim
        treeWaterAnim.SetTrigger("waterGoUp");

        Oka = OkaObj;
        facingRight = OkaObj.transform.position.x > root_trigger.transform.position.x ? false : true;
    }

    //由animation event呼叫
    void TreePassOver()
    {
        if (Oka == null) return;

        //呼叫playerController 讓主角出現並往正確方向跳出去
        Vector3 exitPoint = facingRight ? rightEnd.position : leftEnd.position;
        Oka.GetComponent<PlayerControl>().TreePassOver(exitPoint, facingRight);

        //particleSystem噴一點水
        GameObject FXObj = Instantiate(waterSplashFX, exitPoint, Quaternion.Euler(0f, facingRight ? 0f : 180f, 0f));
        Destroy(FXObj, 2f);

    }

    IEnumerator DestroyFX_Delay(GameObject FXObj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(FXObj);
    }
}
