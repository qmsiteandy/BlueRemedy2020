using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolePass : MonoBehaviour {

    public GameObject holeWater1, holeWater2;
    public Transform firstEnd, secondEnd;
    public float firstEndAngle, secondEndAngle;
    public GameObject waterSplashFX;
    public float FX_delay = 0.3f;

    private Animator holeWaterAnim;
    private GameObject Oka;
    private bool beginFromFirstEnd;
    private Transform player;
    private Skill_Water skill_Water;


    // Use this for initialization
    void Start () {

        player = GameObject.Find("Player").GetComponent<Transform>();
        skill_Water = player.Find("Oka_water").GetComponent<Skill_Water>();

        holeWaterAnim = GetComponent<Animator>();
        holeWater1.SetActive(false); holeWater2.SetActive(false);
    }

    //由Skill_Water呼叫
    public void WaterPassing(GameObject OkaObj)
    {
        beginFromFirstEnd = (OkaObj.transform.position - firstEnd.position).sqrMagnitude < 1.5f * 1.5;

        if (beginFromFirstEnd)
        {
            holeWater1.SetActive(true); holeWater2.SetActive(false);
            skill_Water.SetWaterdropTrans(holeWater1.transform);
        }
        else
        {
            holeWater1.SetActive(false); holeWater2.SetActive(true);
            skill_Water.SetWaterdropTrans(holeWater2.transform);
        }

        holeWaterAnim.SetTrigger("waterGo");

        Oka = OkaObj;
    }

    //由animation event呼叫
    void WaterPassOver()
    {
        if (Oka == null) return;

        holeWater1.SetActive(false); holeWater2.SetActive(false);

        //呼叫playerController 讓主角出現並往正確方向跳出去
        Vector3 exitPoint = beginFromFirstEnd ? secondEnd.position : firstEnd.position;
        float exitAngle = beginFromFirstEnd ? secondEndAngle : firstEndAngle;

        Oka.GetComponent<Skill_Water>().HolePassOver(exitPoint, exitAngle);

        //particleSystem噴一點水
        StartCoroutine(FX_Delay(waterSplashFX, FX_delay, exitPoint, Quaternion.Euler(0f, exitAngle, 0f)));
    }
    IEnumerator FX_Delay(GameObject FX, float delay, Vector3 point, Quaternion angle)
    {
        yield return new WaitForSeconds(delay);
        GameObject FXObj = Instantiate(waterSplashFX, point, angle);
        Destroy(FXObj, 1.5f);
    }

}
