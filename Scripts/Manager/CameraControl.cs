using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform camera_target;            //camera追蹤目標
    public float Yoffset = 2.8f;
    public float smoothSpeed = 0.25f;   //camera移動速度
    public float lookUpMove = 1.2f;     //向上看時camera垂直移動距離
    public float lookDownMove = 3f;     //向下看時camera垂直移動距離

    private Vector3 offset;             //camera與主角相對位置差距
    private Vector3 AimPos;             //camera目標前往位置
    private bool isFollowMode = true;   //是否為一般跟隨模式


    void Start()
    {
        //設定相機初始位置
        transform.position = camera_target.position + new Vector3(0f, Yoffset, 0f);
    }

    void FixedUpdate()
    {
        //如果是followMode，設定camera目標前往位置
        if (isFollowMode) { AimPos = camera_target.position + new Vector3(0f, Yoffset, 0f); }

        //平滑移動camera
        Vector3 smoothPos = Vector3.Lerp(transform.position, AimPos, smoothSpeed);
        transform.position = smoothPos;
    }

    //主角往上看時呼叫此函式
    public void PlayerLookUp()
    {
        //設定目標前往位置往上移
        AimPos += new Vector3(0f, lookUpMove, 0f);
        //停止follow
        isFollowMode = false;
    }

    //主角往下看時呼叫此函式
    public void PlayerLookDown()
    {
        //設定目標前往位置往下移
        AimPos -= new Vector3(0f, lookDownMove, 0f);
        //停止follow
        isFollowMode = false;
    }

    //設定camera目標位置並停止follow主角
    public void SetAimPos(float xAim, float yAim)
    {
        //設定camera目標位置
        AimPos = new Vector3(xAim, yAim, 0f);
        //停止follow
        isFollowMode = false;
    }

    //回歸follow主角
    public void BackNormal()
    {
        isFollowMode = true;
    }

    public void SetTarget(Transform newTraget)
    {
        camera_target = newTraget;
    }
    public void SetTarget(Transform newTraget,float delay)
    {
        StartCoroutine(SetTarget_Delay(newTraget, delay));
    }
    IEnumerator SetTarget_Delay(Transform newTraget, float delay)
    {
        yield return (new WaitForSeconds(delay));
        camera_target = newTraget;
    }
}
