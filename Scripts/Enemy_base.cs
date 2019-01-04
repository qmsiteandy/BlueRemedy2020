using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_base : MonoBehaviour {
    
    GameObject target;
    Animator animator;
    SpriteRenderer spriteRenderer;  //怪物圖的render

    public float moveRange = 3f;    //移動範圍半徑
    public float moveSpeed = 1.5f;  //移動速度
    public float awakeTime = 1.5f;  //重新醒來時間
    public bool isAwake;            //是否醒著
    public float trackSpeed = 1.5f;

    private Vector2 centerPos;      //移動的區域中點
    private float posNow = 0f;      //目前移動相對中點的位置
    private bool goRight = true;    //是否往右走
    private float awakeCountdown = 0f;  //重新清醒時間倒數
    public bool isTracking = false;
    public bool isAttacking;
    public bool isAlert;
    

    // Use this for initialization
    void Awake()
    {

        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        //設定為清醒
        isAwake = true;
        //移動中點設為最初的位置
        centerPos = transform.position;
    }

    void Start()
    {
        //if (Input.GetKeyDown(KeyCode.Z)) animator.SetTrigger("daze");
    }

    void Update()
    {
        //是否醒著
        if (!isAwake)
        {
            //倒數
            awakeCountdown -= Time.deltaTime;
            //倒數歸零時重新醒來
            if (awakeCountdown <= 0) isAwake = true;
        }

        if (isTracking)
        {
            Tracking();
        }
        else
        {
            MoveAround();
        }
    }

    //來回移動
    void MoveAround()
    {
        if (target == null && !isAttacking )
        {
            //設定posNow隨時間增加&減少

            //如果posNow超出範圍
            if (posNow >= moveRange || posNow <= -moveRange)
            {
                //設定posNow到範圍邊界
                posNow = Mathf.Clamp(posNow, -moveRange, moveRange);
                //往回走
                goRight = !goRight;
                if (goRight)
                {
                    transform.localScale = new Vector3(1,1,1);
                }
                else {
                    transform.localScale = new Vector3(-1,1,1);
                }
                    
            }
            if (goRight) posNow += moveSpeed * Time.deltaTime;
            else if (!goRight) posNow -= moveSpeed * Time.deltaTime;

            //設定腳色的位置
            transform.position = new Vector3(centerPos.x + posNow, centerPos.y, 0f);


        }
    }

    //公開函式，當被踩踏時
    public void IsSteppedOn()
    {
        if (isAwake)
        {
            //昏厥
            animator.SetTrigger("daze");
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            target = collision.gameObject;

            isTracking = true;
        }
    }

    void Tracking()
    {
        if (target != null && !isAttacking && !isAlert)
        {
            
            Vector3 diff = new Vector3(target.transform.position.x - transform.position.x, 0, 0);

            if (Vector2.SqrMagnitude(new Vector2(diff.x, 0)) <=0.25)
            {
                animator.SetTrigger("alert");
                isAlert = true;
                
            }
            else{
                posNow = Mathf.Lerp(posNow, target.transform.position.x, trackSpeed * Time.deltaTime);
                posNow = Mathf.Clamp(posNow, -moveRange, moveRange);
                transform.position = new Vector3(centerPos.x + posNow, centerPos.y, 0f);

                float face = Mathf.Sign(diff.x);
                goRight = (face >= 0) ? true : false;
                Vector3 faceVec = new Vector3(face, 1, 1);
                transform.localScale = faceVec;
           }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
            target = null;
            isTracking = false;

        }

    }

    public void DazeOver()
    {
        isAwake = true;
    }

    public void AttackOver()
    {
        isAttacking = false;
    }

    public void AlertOver()
    {
        isAlert = false;
    }


}
