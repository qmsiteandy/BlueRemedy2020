using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolePass : MonoBehaviour {

    public GameObject holeWater1, holeWater2;
    public Transform firstEnd, secondEnd;
    public float firstEndAngle, secondEndAngle;
    public GameObject waterSplashFX;
    public CameraControl cameraControl;

    private Animator holeWaterAnim;
    private GameObject Oka;
    private bool beginFromFirstEnd;


    // Use this for initialization
    void Start () {

        holeWaterAnim = GetComponent<Animator>();
        holeWater1.SetActive(false); holeWater2.SetActive(false);
    }

    //由playerController呼叫
    public void WaterPassing(GameObject OkaObj)
    {
        beginFromFirstEnd = (OkaObj.transform.position - firstEnd.position).sqrMagnitude < 1.5f * 1.5;

        if (beginFromFirstEnd)
        {
            holeWater1.SetActive(true); holeWater2.SetActive(false);
            //設定相機跟隨目標切換
            cameraControl.SetTarget(holeWater1.transform);
        }else
        {
            holeWater1.SetActive(false); holeWater2.SetActive(true);
            //設定相機跟隨目標切換
            cameraControl.SetTarget(holeWater2.transform);
        }

        holeWaterAnim.SetTrigger("waterGo");

        Oka = OkaObj;
    }

    //由animation event呼叫
    void WaterPassOver()
    {
        if (Oka == null) return;

        //呼叫playerController 讓主角出現並往正確方向跳出去
        Vector3 exitPoint = beginFromFirstEnd ? secondEnd.position : firstEnd.position;
        float exitAngle = beginFromFirstEnd ? secondEndAngle : firstEndAngle;

        Oka.GetComponent<PlayerControl>().HolePassOver(exitPoint, exitAngle);

        //particleSystem噴一點水
        GameObject FXObj = Instantiate(waterSplashFX, exitPoint, Quaternion.Euler(0f, exitAngle, 0f));
        Destroy(FXObj, 1.5f);
    }
}
