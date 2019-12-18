using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    [Header("基本參數")]
    [Range(0, 2)]static public int OkaID_Now = 1;
    public float initSpeedLimit = 8.0f;
    [HideInInspector] static public float speedLimit;           //移動速度上限
    public float accelForce = 80f;       //加速時間
    public float jumpForce = 650.0f;    //跳躍力道
    public float walljumpForce = 750.0f;
    [HideInInspector] public Rigidbody2D rb2d;          //儲存主角的Rigidbody2D原件
    [HideInInspector] static public bool canMove = true;
    [HideInInspector] static public bool facingRight = true;    //是否面向右
    [HideInInspector] static public float xSpeed = 0f;
    private PlayerEnergy playerEnergy;
    private PlayerChange playerChange;

    [Header("跳躍判斷")]
    public Transform footCheck;         //檢查踩踏地板的點
    public Transform frontCheck, backCheck;
    public float checkRadius = 0.4f;    //檢查踩踏地板的判斷半徑
    private LayerMask whatIsGround;      //檢查踩踏地板的地板圖層
    private LayerMask whatIsPlatform;
    private LayerMask whatIsWall;
    [HideInInspector] static public bool footLanding = false;
    private bool onGround, onPlatform;
    private bool frontTouchWall = false, backTouchWall = false;
    private bool secondJumping = false;
    private bool pressingJump = false;
    private bool jumpingDown = false;
    private float shortCantMoveDuration = 0.08f;

    [Header("三態個別")]
    private SpriteRenderer[] spriteRenderer = { null, null, null };  //用來設定sorting
    private Animator[] animator = { null, null, null };
    private Skill_Base[] skill_Base = { null, null, null };
    private Skill_Water skill_Water;

  
    [Header("機關設定")]
    public GameObject noticeUI;
    private Transform noticeUI_Trans;
    public CameraControl cameraControl;

    [Header("水中")]
    public ParticleSystem bubbleMaker;
    private bool isBubbling = false;
    private int DirtyWaterLayerID = 14;
    private float iceFloatForce = 50f;
    public bool isInWater = false;
    private DirtyWater dirtyWater;
    public GameObject waterSplashFX;
    


    void Start()
    {
        cameraControl = GameObject.Find("CameraHolder").GetComponent<CameraControl>();

        rb2d = GetComponent<Rigidbody2D>();
        playerEnergy = this.GetComponent<PlayerEnergy>();
        playerChange = this.GetComponent<PlayerChange>();

        for (int x = 0; x < 3; x++)
        {
            //Transform child = this.transform.GetChild(x);
            spriteRenderer[x] = this.transform.GetChild(x).GetComponent<SpriteRenderer>();
            animator[x] = this.transform.GetChild(x).GetComponent<Animator>();

            skill_Base[x] = this.transform.GetChild(x).GetComponent<Skill_Base>();
        }
        skill_Water = this.transform.GetChild(1).GetComponent<Skill_Water>();

        speedLimit = initSpeedLimit;


        //---各種圖層MASK設定---
        whatIsGround = LayerMask.GetMask("Ground");
        whatIsPlatform = LayerMask.GetMask("Platform");
        whatIsWall = LayerMask.GetMask("Wall");

        //---NoticeMark--
        noticeUI.SetActive(false);
        noticeUI_Trans = noticeUI.GetComponent<Transform>();

        //---bubble maker init---
        bubbleMaker = transform.Find("BubbleMaker").GetComponent<ParticleSystem>();
        bubbleMaker.Stop();
    }

    void FixedUpdate()
    {
        if(!isInWater) PointCheck();
        
        Move();
        Jump();

    }

    void PointCheck()
    {
        //以半徑圓範圍偵測是否在地上，儲存到grounded
        onGround = Physics2D.OverlapCircle(footCheck.position, checkRadius, whatIsGround);
        onPlatform = Physics2D.OverlapCircle(footCheck.position, checkRadius, whatIsPlatform);
        footLanding = onGround || onPlatform;
        frontTouchWall = Physics2D.OverlapCircle(frontCheck.position, 0.1f, whatIsWall);
        backTouchWall = Physics2D.OverlapCircle(backCheck.position, 0.35f, whatIsWall);
        if (footLanding || frontTouchWall || backTouchWall) secondJumping = false;
    }

    #region ================↓移動轉身相關↓================
    void Move()
    {
        float xInput;

        if (canMove)
        {
            xInput = Input.GetAxis("Horizontal");

            Vector2 force = new Vector2(xInput * accelForce, 0f);
            rb2d.AddForce(force);
        }
        else xInput = 0f;

        if (xInput != 0) rb2d.velocity = new Vector3(Mathf.Clamp(rb2d.velocity.x, xInput * speedLimit, xInput * speedLimit), rb2d.velocity.y, 0f);
        else
        {
            if (Mathf.Abs(rb2d.velocity.x) > 0.4f) rb2d.velocity = new Vector3(rb2d.velocity.x * 0.9f, rb2d.velocity.y, 0f);
            else rb2d.velocity = new Vector3(0f, rb2d.velocity.y, 0f);
        }

        animator[OkaID_Now].SetFloat("xSpeed", Mathf.Abs(rb2d.velocity.x));

        if (xInput != 0f)
        {
            if ((facingRight && rb2d.velocity.x < 0f) || (!facingRight && rb2d.velocity.x > 0f)) Flip();
        }

        xSpeed = rb2d.velocity.x;
    }
    void Flip() //偵測移動方向及是否需轉面
    {
        //bool變數代表方向
        facingRight = !facingRight;
        //取得目前物件的規格
        Vector3 theScale = transform.localScale;
        //使規格的水平方向相反
        theScale.x *= -1;
        //套用翻面後的規格
        transform.localScale = theScale;

        UI_Flip(); 
    }
    void UI_Flip()//避免主角轉身時連UI也轉了，所以要再轉一次
    {
        
        //取得目前物件的規格
        Vector3 theScale = noticeUI_Trans.localScale;
        //使規格的水平方向相反
        theScale.x *= -1;
        //套用翻面後的規格
        noticeUI_Trans.localScale = theScale;

        playerChange.WheelUI_Flip();
    }
    #endregion ================↑移動轉身相關↑================

    #region ================↓跳躍相關↓================
    void Jump()
    {
        if (!canMove) return;

        //平台上往下跳
        if (onPlatform && Input.GetButtonDown("Vertical") && !jumpingDown)
        {
            StartCoroutine(JumpDownFromPlat());
            return;
        }
        if (Input.GetAxis("Jump") > 0.1f && !pressingJump) 
        {
            pressingJump = true;
                        
            //在地上
            if (footLanding)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);

                if (!isInWater) rb2d.AddForce(Vector2.up * jumpForce);
                else rb2d.AddForce(Vector2.up * jumpForce * 0.8f);
            }
            //蹬牆跳
            else if (frontTouchWall)
            {
                if (facingRight) { rb2d.AddForce(new Vector2(-walljumpForce * 0.8f, walljumpForce)); }
                else { rb2d.AddForce(new Vector2(walljumpForce * 0.8f, walljumpForce)); }

                StartCoroutine(ShortCantMove(shortCantMoveDuration));
            }
            else if (backTouchWall)
            {
                if (facingRight) { rb2d.AddForce(new Vector2(walljumpForce * 0.8f, walljumpForce)); }
                else { rb2d.AddForce(new Vector2(-walljumpForce * 0.8f, walljumpForce)); }

                StartCoroutine(ShortCantMove(shortCantMoveDuration));
            }
            //二段跳
            else if (!secondJumping && OkaID_Now == 2)
            {
                if (rb2d.velocity.y < 0) rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);

                if (!isInWater) rb2d.AddForce(Vector2.up * jumpForce);
                else rb2d.AddForce(Vector2.up * jumpForce * 0.8f);

                secondJumping = true;               
            }
        }
        //當跳躍鍵放開且此時未著地
        else if (Input.GetAxis("Jump") < 1f && !footLanding)
        {
            //呼叫JumpRelease函示
            JumpRelease();
        }

        if (Input.GetAxis("Jump") < 0.1f) pressingJump = false;
    }
    //彈跳釋放(會影響長按短按地跳躍高度)
    void JumpRelease()
    {
        { 
            //若主角已經是下降狀態就直接離開函式
            if (rb2d.velocity.y <= 0) return;

            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.5f);
        }
    }
    //平台下跳
    IEnumerator JumpDownFromPlat()
    {
        float colDisTime = 0.2f;

        Physics2D.IgnoreLayerCollision(8, 13, true);
        jumpingDown = true;

        yield return new WaitForSeconds(colDisTime);

        Physics2D.IgnoreLayerCollision(8, 13, false);
        jumpingDown = false;
    }
    #endregion ================↑跳躍相關↑================

    #region ================↓trigger相關↓================
    void OnTriggerEnter2D(Collider2D collider)
    {
        //---水中---
        if (collider.gameObject.layer == DirtyWaterLayerID)
        {
            dirtyWater = collider.GetComponent<DirtyWater>();

            //跳入水面的水花
            if (rb2d.velocity.y < -1.5f)
            {
                float splashScale = Mathf.Lerp(0.2f, 0.8f, rb2d.velocity.y / -20f);

                GameObject splash = Instantiate(waterSplashFX, transform.position + new Vector3(0, 0.75f * splashScale, 0f), Quaternion.identity);

                Vector3 oriScale = splash.GetComponent<Transform>().localScale;
                splash.GetComponent<Transform>().localScale = oriScale * splashScale;

                Destroy(splash, 1f);
            }
        }
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        //---滲透毛細提示UI&起始呼叫---
        if (collider.gameObject.tag == "WaterPassingTrigger")
        {
            if (OkaID_Now == 1 && !skill_Water.isPassing)
            {
                noticeUI.SetActive(false);

                if (collider.name == "root_trigger") noticeUI.SetActive(true);
                else if ((collider.name == "leftEnd" && facingRight) || (collider.name == "rightEnd" && !facingRight)) noticeUI.SetActive(true);
                else return;

                skill_Water.PassBegin(collider);
            }
            else noticeUI.SetActive(false);
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        //---水中---
        if (collider.gameObject.layer == DirtyWaterLayerID)
        {
            //跳出水面的水花
            if (rb2d.velocity.y > 0.3f)
            {
                float splashScale = Mathf.Lerp(0.1f, 0.4f, rb2d.velocity.y / 10f);

                GameObject splash = Instantiate(waterSplashFX, transform.position - new Vector3(0, 0.75f * splashScale, 0f), Quaternion.identity);

                Vector3 oriScale = splash.GetComponent<Transform>().localScale;
                splash.GetComponent<Transform>().localScale = oriScale * splashScale;

                Destroy(splash, 1f);
            }
        }

        //---滲透&毛細提醒UI---
        else if (collider.gameObject.tag == "WaterPassingTrigger") noticeUI.SetActive(false);

    }
    #endregion ================↑trigger相關↑================

    #region ================↓受傷復原相關↓================
    //---受傷(多載:同時or個別設定水量髒污的影響)---
    public void TakeDamage(int damage)
    {
        if (skill_Water.isPassing) skill_Water.PassingInterrupted();
        skill_Base[OkaID_Now].BackIdle();

        playerEnergy.ModifyDirt(damage);
        playerEnergy.ModifyWaterEnergy(-damage);

        animator[OkaID_Now].SetTrigger("gotHurt");
        StartCoroutine(ShortCantMove(0.2f));

        StartCoroutine(DamagedColor());
    }
    public void TakeDamage(int waterCost,int addDirt)
    {
        if (skill_Water.isPassing) skill_Water.PassingInterrupted();
        skill_Base[OkaID_Now].BackIdle();

        playerEnergy.ModifyDirt(addDirt);
        playerEnergy.ModifyWaterEnergy(- waterCost);

        animator[OkaID_Now].SetTrigger("gotHurt");
        StartCoroutine(ShortCantMove(0.2f));

        StartCoroutine(DamagedColor());
    }
    IEnumerator DamagedColor()
    {
        spriteRenderer[OkaID_Now].color = new Color(0.7f, 0f, 0f);

        yield return new WaitForSeconds(0.08f);

        spriteRenderer[OkaID_Now].color = new Color(1f, 1f, 1f);
    }
    //---復原---
    public void TakeHeal(int waterHeal, int dirtHeal)
    {
        playerEnergy.ModifyWaterEnergy(waterHeal);
        playerEnergy.ModifyDirt(-dirtHeal);
    }
    #endregion ================↑受傷復原相關↑================

    IEnumerator ShortCantMove(float duration)
    {
        float timer = 0f;

        canMove = false;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return 0;
        }
        canMove = true;
    }

    //---水中---
    public void InWater()
    {
        //---水中漂浮&冒泡泡---
        
        footLanding = true;

        if (OkaID_Now == 0)
        {
            if (dirtyWater.waveCrest - transform.position.y > 0.8f) rb2d.AddForce(Vector2.up * iceFloatForce);
            else if (dirtyWater.waveCrest - transform.position.y > 0f) rb2d.AddForce(Vector2.up * iceFloatForce * (dirtyWater.waveCrest - transform.position.y / 1f));

            if(isBubbling) bubbleMaker.Stop();
        }
        else
        {
            float depth = dirtyWater.waveCrest - transform.position.y;
            if (depth > 0.5f) bubbleMaker.startLifetime = depth * 0.8f;

            if (depth > 0.5f && !isBubbling) { bubbleMaker.Play(); isBubbling = true; }
            else if (depth < 0.5f && isBubbling) { bubbleMaker.Stop(); isBubbling = false; }
        } 
    }

    //關閉UI
    public void TrnasformReset()
    {
        for(int x = 0; x < 3; x++)
        {
            spriteRenderer[x].color = new Color(1f, 1f, 1f);
        }
    }

    public void SetPlayerPosLerp(Vector3 GoalPos, float time)
    {
        Vector3 pos = Vector3.Lerp(this.transform.position, GoalPos, time);
        transform.position = pos;
    }
}
