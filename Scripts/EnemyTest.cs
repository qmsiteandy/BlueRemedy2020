using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{

    public float moveRange = 3f;    //移動範圍半徑
    public float moveSpeed = 1.5f;  //移動速度
    public float awakeTime = 1.5f;  //重新醒來時間
    public bool isAwake;            //是否醒著

    private Vector2 centerPos;      //移動的區域中點
    private float posNow = 0f;      //目前移動相對中點的位置
    private bool goRight = true;    //是否往右走
    private float awakeCountdown = 0f;  //重新清醒時間倒數

    private SpriteRenderer spriteRenderer;  //怪物圖的render

	// Use this for initialization
	void Awake () {

        spriteRenderer = GetComponent<SpriteRenderer>();

        //設定為清醒
        isAwake = true;
        //移動中點設為最初的位置
        centerPos = transform.position;
    }
	

	void Update ()
    {
        //是否醒著
        if (isAwake)
        {
            //來回移動
            MoveAround();
        }

        else if (!isAwake)
        {
            //倒數
            awakeCountdown -= Time.deltaTime;
            //倒數歸零時重新醒來
            if (awakeCountdown <= 0) isAwake = true;
        }
	}

    //來回移動
    void MoveAround()
    {
        //設定posNow隨時間增加&減少
        if (goRight) posNow += moveSpeed * Time.deltaTime;
        else if(!goRight) posNow -= moveSpeed * Time.deltaTime;

        //如果posNow超出範圍
        if (posNow > moveRange || posNow < -moveRange)
        {
            //設定posNow到範圍邊界
            posNow = Mathf.Clamp(posNow, -moveRange, moveRange);
            //往回走
            goRight = !goRight;
        }

        //設定腳色的位置
        transform.position = new Vector3(centerPos.x+posNow, centerPos.y, 0f);
    }

    //公開函式，當被踩踏時
    public void IsSteppedOn()
    {
        if (isAwake)
        {
            //昏厥
            isAwake = false;
            //設定倒數計時的變數內容為重新清醒時間
            awakeCountdown = awakeTime;

            StartCoroutine(ChangeColor(new Color(1, 0, 0), 0.05f));
        }
    }

    IEnumerator ChangeColor(Color color, float colorChangeTime)
    {
        float elapsed = 0f;

        while (elapsed < colorChangeTime)
        {
            spriteRenderer.color = color;

            elapsed += Time.deltaTime;

            yield return null;
        }
        spriteRenderer.color = new Color(1, 1, 1);
    }
}
