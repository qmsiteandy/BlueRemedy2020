using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_base : MonoBehaviour {

    GameObject target;
    Animator animator;
    SpriteRenderer spriteRenderer;  //怪物圖的render
    Vector2 centerPos;      //移動的區域中點
    public GameObject enemy;

    [Header("Move Settings")]
    public float moveRange = 3f;    //移動範圍半徑
    public float moveSpeed = 1.5f;  //移動速度
    private float posNow = 0f;      //目前移動相對中點的位置
    private bool goRight = true;    //是否往右走
    public float trackSpeed = 1.5f;
    public float closeRange = 1.25f;

    [Header("Awake Settings")]
    public float bornTime = 5f;

    [Header("Health Settings")]
    public int health;
    public int healthMax = 10;

    [Header("Action Settings")]
    public bool isTracking = false;
    public bool isAttacking;
    public bool isAlert;
    public bool isBeAttack;
    public bool isDie;
    public bool isBorn;


    // Use this for initialization
    void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        //animator = GetComponent<Animator>();
        centerPos = transform.position; //移動中點設為最初的位置
    }

    void Start()
    {
        health = healthMax;
        enemy = transform.GetChild(0).gameObject;
        //if (Input.GetKeyDown(KeyCode.Z)) animator.SetTrigger("daze");
    }

    void Update()
    {
        if (isTracking)
        {
            Tracking();
        }
        else
        {
            MoveAround();
        }

        //if (isBorn)
        //{
        //    bornTime -= Time.deltaTime; //重生倒數
        //    if (bornTime <= 0)
        //    {
        //        isDie = false;
        //        animator.SetTrigger("rebirth");
        //        enemy.SetActive(true);
        //        isBorn = false;
        //    }
        //}
    }

    //來回移動
    void MoveAround()
    {
        if (target == null && !isAttacking)
        {
            //如果posNow超出範圍
            if (posNow >= moveRange || posNow <= -moveRange)
            {
                posNow = Mathf.Clamp(posNow, -moveRange, moveRange); //設定posNow到範圍邊界
                goRight = !goRight; //往回走
                if (goRight)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }

            if (goRight) posNow += moveSpeed * Time.deltaTime;
            else if (!goRight) posNow -= moveSpeed * Time.deltaTime;

            transform.position = new Vector3(centerPos.x + posNow, centerPos.y, 0f); //設定腳色的位置
        }
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
        if (target != null && !isAttacking)
        {

            Vector3 diff = new Vector3(target.transform.position.x - transform.position.x, 0, 0);
            if (Mathf.Abs(diff.x) <= closeRange) return;
            /*if (Vector2.SqrMagnitude(new Vector2(diff.x, 0)) <=0.25)
            {
                animator.SetTrigger("alert");
                isAlert = true;
                
            }*/
            //else{
            posNow = Mathf.Lerp(posNow, target.transform.position.x - centerPos.x, trackSpeed * Time.deltaTime);
            posNow = Mathf.Clamp(posNow, -moveRange, moveRange);
            transform.position = new Vector3(centerPos.x + posNow, centerPos.y, 0f);

            float face = Mathf.Sign(diff.x);
            goRight = (face >= 0) ? true : false;
            Vector3 faceVec = new Vector3(face, 1, 1);
            transform.localScale = faceVec;
            // }

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
    public void TakeDamage(int damage)
    {
        if (!isDie)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                //animator.SetTrigger("die");
            }
            //animator.SetTrigger("beattack");
            isBeAttack = true;

            StartCoroutine(RedFlash());
        }
    }

    IEnumerator RedFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}


