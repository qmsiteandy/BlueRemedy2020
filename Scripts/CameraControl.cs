using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform target;            //camera追蹤目標
    public float smoothSpeed = 0.05f;   //camera移動速度
    public float lookUpMove = 1.5f;     //向上看時camera垂直移動距離
    public float lookDownMove = 4f;     //向下看時camera垂直移動距離

    private Vector3 offset;             //camera與主角相對位置差距
    private Vector3 AimPos;             //camera目標前往位置
    bool isFollowMode = true;           //是否為一般跟隨模式
    

	void Start ()
    {
        //設定offset為初始主角及初始相機位置之差距
        offset = transform.position - target.position; 
	}
	
	void FixedUpdate ()
    {
        //如果是followMode，設定camera目標前往位置
        if (isFollowMode) { AimPos = target.position + offset; }   

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
}
