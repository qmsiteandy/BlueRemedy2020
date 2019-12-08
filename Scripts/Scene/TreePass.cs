using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePass : MonoBehaviour {

    public GameObject treeWater;
    public GameObject root_trigger;
    public Transform rightEnd, leftEnd;
    public GameObject waterSplashFX;
    public float FX_delay = 0.3f;

    private Animator treeWaterAnim;
    private GameObject Oka;
    private bool facingRight;
    private Transform player;
    private Skill_Water skill_Water;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        skill_Water = player.Find("Oka_water").GetComponent<Skill_Water>();

        treeWaterAnim = GetComponent<Animator>();
    }
	
    //由Skill_Water呼叫
	public void WaterPassing(GameObject OkaObj)
    {
        skill_Water.SetWaterdropTrans(treeWater.transform);

        //樹中水毛細上升anim
        treeWaterAnim.SetTrigger("waterGo");

        Oka = OkaObj;
        facingRight = OkaObj.transform.position.x > root_trigger.transform.position.x ? false : true;
    }

    //由animation event呼叫
    void WaterPassOver()
    {
        if (Oka == null) return;

        //呼叫playerController 讓主角出現並往正確方向跳出去
        Vector3 exitPoint = facingRight ? rightEnd.position : leftEnd.position;
        Oka.GetComponent<Skill_Water>().TreePassOver(exitPoint, facingRight);

        //particleSystem噴一點水
        StartCoroutine(FX_Delay(waterSplashFX, FX_delay, exitPoint, Quaternion.Euler(0f, facingRight ? 0f : 180f, 0f)));
    }

    IEnumerator FX_Delay(GameObject FX, float delay,Vector3 point,Quaternion angle)
    {
        yield return new WaitForSeconds(delay);
        GameObject FXObj = Instantiate(waterSplashFX, point, angle);
        Destroy(FXObj, 1.5f);
    }
}
