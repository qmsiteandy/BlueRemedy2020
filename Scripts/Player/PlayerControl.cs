using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    [Header("基本參數")]
    [Range(0, 2)]static public int OkaID_Now = 1;
    private float xInput;
    public float initSpeedLimit = 8.0f;
    [HideInInspector] static public float speedLimit;           //移動速度上限
    public float accelForce = 80f;       //加速時間
    public float jumpForce = 650.0f;    //跳躍力道
    public float walljumpForce = 750.0f;
    [HideInInspector] public Rigidbody2D rb2d;          //儲存主角的Rigidbody2D原件
    [HideInInspector] static public bool facingRight = true;    //是否面向右
    [HideInInspector] static public float xSpeed = 0f;
    private PlayerEnergy playerEnergy;
    private PlayerChange playerChange;
    private float angleWithCol = 0f;    //與Ground & Wall 夾角
    private float GroundWall_angleLimit = 35f;

    [Header("跳躍判斷")]
    private Transform footCheck;         //檢查踩踏地板的點
    private Transform frontCheck, backCheck;
    public float checkRadius = 0.4f;    //檢查踩踏地板的判斷半徑
    private LayerMask whatIsGround;      //檢查踩踏地板的地板圖層
    private LayerMask whatIsPlatform;
    [HideInInspector] static public bool footLanding = false;
    private bool onGround, onPlatform;
    private bool frontTouchWall = false, backTouchWall = false;
    private bool touchGround;
    [HideInInspector] public bool jumping = false;
    private bool secondJumping = false;
    private bool pressingJump = false;
    private bool jumpingDown = false;
    public bool isStickOnWall = false;
    public GameObject jumptwice_deco;

    [Header("三態個別")]
    private SpriteRenderer[] spriteRenderer = { null, null, null };  //用來設定sorting
    private Animator[] animator = { null, null, null };
    private Skill_Base[] skill_Base = { null, null, null };
    private Skill_Water skill_Water;
  
    [Header("機關設定")]
    [HideInInspector] public GameObject noticeUI;
    private Transform noticeUI_Trans;
    private CameraControl cameraControl;

    [Header("水中")]
    public ParticleSystem bubbleMaker;
    private bool isBubbling = false;
    private int DirtyWaterLayerID = 14;
    private float iceFloatForce = 50f;
    public bool isInWater = false;
    private Water_Area water_area;
    public GameObject waterSplashFX;

    [Header("髒污ripple")]
    private SpriteMask rippleMask;

    void Awake()
    {
        cameraControl = GameObject.Find("CameraHolder").GetComponent<CameraControl>();

        rb2d = GetComponent<Rigidbody2D>();
        playerEnergy = this.GetComponent<PlayerEnergy>();
        playerChange = this.GetComponent<PlayerChange>();

        footCheck = transform.Find("footPoint");
        frontCheck = transform.Find("frontPoint");
        backCheck = transform.Find("backPoint");

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
        whatIsGround = LayerMask.GetMask("Ground & Wall");
        whatIsPlatform = LayerMask.GetMask("Platform");

        //---NoticeMark--
        noticeUI.transform.Find("notice mark");
        noticeUI.SetActive(false);
        noticeUI_Trans = noticeUI.GetComponent<Transform>();

        //---bubble maker init---
        bubbleMaker = transform.Find("BubbleMaker").GetComponent<ParticleSystem>();
        bubbleMaker.Stop();

        //---用於髒污ripple的材質遮罩設定---
        rippleMask = this.GetComponent<SpriteMask>();
    }

    void FixedUpdate()
    {
        //if(!isInWater) PointCheck();
        PointCheck();

        Move();
        Jump();
        WallStick();
    }

    void Update()
    {
        //---用於髒污塗層的遮罩---
        rippleMask.sprite = spriteRenderer[OkaID_Now].sprite;
    }

    void PointCheck()
    {
        //以半徑圓範圍偵測是否在地上，儲存到grounded
        onGround = Mathf.Abs(angleWithCol) >= GroundWall_angleLimit && Physics2D.OverlapCircle(footCheck.position, checkRadius, whatIsGround);
        onPlatform = Mathf.Abs(angleWithCol) >= GroundWall_angleLimit && Physics2D.OverlapCircle(footCheck.position, checkRadius, whatIsPlatform);

        footLanding = onGround || onPlatform || isInWater;
        frontTouchWall = Physics2D.OverlapCircle(frontCheck.position, 0.45f, whatIsGround);
        backTouchWall = Physics2D.OverlapCircle(backCheck.position, 0.35f, whatIsGround);

        touchGround = (footLanding || frontTouchWall || backTouchWall);

        if (footLanding || frontTouchWall || backTouchWall) secondJumping = false; 
    }

    void WallStick()
    {
        //if (!isStickOnWall && Mathf.Abs(xInput) > 0f && frontTouchWall && !footLanding)
        //{
        //    isStickOnWall = true;
        //    animator[OkaID_Now].SetBool("stickOnWall", isStickOnWall);
        //}
        //else if(isStickOnWall && (!frontTouchWall || Mathf.Abs(xInput) == 0f || footLanding))
        //{
        //    isStickOnWall = false;
        //    animator[OkaID_Now].SetBool("stickOnWall", isStickOnWall);
        //}
    }

    #region ================↓移動轉身相關↓================
    void Move()
    {
        if (PlayerStatus.canMove)
        {
            xInput = Input.GetAxis("Horizontal") + Input.GetAxis("XBOX_Horizontal");

            Vector2 force = new Vector2(xInput * accelForce, 0f);
            rb2d.AddForce(force);
        }

        if (!frontTouchWall) rb2d.velocity = new Vector3(Mathf.Clamp(rb2d.velocity.x, xInput * speedLimit, xInput * speedLimit), rb2d.velocity.y, 0f);
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
        if (!PlayerStatus.canJump) return;

        animator[OkaID_Now].SetBool("touchGroung", touchGround);
        jumping = !touchGround;

        animator[OkaID_Now].SetBool("jumpFall", false);
        animator[OkaID_Now].SetBool("jumpUp", false);

        if (jumping)
        {
            if(rb2d.velocity.y < -0.1f) animator[OkaID_Now].SetBool("jumpFall", true);
            if (rb2d.velocity.y > 0.1f) animator[OkaID_Now].SetBool("jumpUp", true);
        }


        //平台上往下跳
        if (onPlatform && (Input.GetAxis("Vertical") < 0f || Input.GetAxis("XBOX_Vertical") > 0.5f) && !jumping)
        {
            StartCoroutine(JumpDownFromPlat());

            return;
        }
        if (Input.GetAxis("Jump") > 0.1f && !pressingJump) 
        {
            pressingJump = true;
            
            //在地上
            if (footLanding && !jumping)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);

                if (!isInWater) rb2d.AddForce(Vector2.up * jumpForce);
                else rb2d.AddForce(Vector2.up * jumpForce * 0.8f);
            }
            //蹬牆跳
            else if (frontTouchWall && !footLanding && !jumping)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);

                if (facingRight) { rb2d.AddForce(new Vector2(-walljumpForce * (xInput>0f ? 2.5f : 0.8f), walljumpForce)); }
                else { rb2d.AddForce(new Vector2(walljumpForce * (xInput < 0f ? 2.5f : 0.8f), walljumpForce)); }

                StartCoroutine(ShortCantMove(0.8f));
            }
            else if (backTouchWall && !footLanding && !jumping)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);

                if (facingRight) { rb2d.AddForce(new Vector2(walljumpForce * 0.8f, walljumpForce)); }
                else { rb2d.AddForce(new Vector2(-walljumpForce * 0.8f, walljumpForce)); }

                StartCoroutine(ShortCantMove(0.8f));
            }
            //二段跳
            else if (!secondJumping && OkaID_Now == 2)
            {
                //if (rb2d.velocity.y < 0) rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
                rb2d.velocity *= new Vector2(1f, 0f);

                if (!isInWater) rb2d.AddForce(Vector2.up * jumpForce);
                else rb2d.AddForce(Vector2.up * jumpForce * 0.6f);

                secondJumping = true;

                animator[OkaID_Now].SetTrigger("gas_jumpTwice");

                GameObject FX = Instantiate(jumptwice_deco, transform.position + new Vector3(0, 0, 0f), Quaternion.identity);
                Destroy(FX, 1f);
            }
        }
        ////當跳躍鍵放開且此時未著地
        //else if (Input.GetAxis("Jump") < 1f && !footLanding)
        //{
        //    //呼叫JumpRelease函示
        //    JumpRelease();
        //}

        //輸入為零
        if (Input.GetAxis("Jump") < 0.1f) pressingJump = false;

        PlayerStatus.isLanding = touchGround;
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
        //jumpingDown = true;

        yield return new WaitForSeconds(colDisTime);

        Physics2D.IgnoreLayerCollision(8, 13, false);
        //jumpingDown = false;
    }
    #endregion ================↑跳躍相關↑================

    #region ================↓trigger相關↓================
    void OnTriggerEnter2D(Collider2D collider)
    {
        //---水中---
        if (collider.gameObject.layer == DirtyWaterLayerID)
        {
            water_area = collider.GetComponent<Water_Area>();
            for (int x = 0; x < 3; x++) { spriteRenderer[x].sortingLayerName = "Scene"; spriteRenderer[x].sortingOrder = -1; }

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

                if (collider.name == "root_trigger" ||
                 ((collider.name == "firstEnd" && facingRight) || (collider.name == "secondEnd" && !facingRight)))
                {
                    noticeUI.SetActive(true);
                    PlayerStatus.isInInteractTrigger = true;
                }
                skill_Water.WaitPassInput(collider);
            }
            else noticeUI.SetActive(false);
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        //---水中---
        if (collider.gameObject.layer == DirtyWaterLayerID)
        {
            for (int x = 0; x < 3; x++) { spriteRenderer[x].sortingLayerName = "Player"; spriteRenderer[x].sortingOrder = 0; }
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
        else if (collider.gameObject.tag == "WaterPassingTrigger")
        {
            noticeUI.SetActive(false);
            PlayerStatus.isInInteractTrigger = false;
        }
    }
    #endregion ================↑trigger相關↑================

    #region ================↓collider相關↓================
    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 9 || collision.gameObject.layer == 13)
        {
            Vector2 contactNormal = collision.GetContact(0).normal; //取得交點法向量  
            angleWithCol = (Mathf.Atan(contactNormal.y / contactNormal.x)) * 180f / Mathf.PI; //計算角度

            //Debug.Log(angleWithCol);

            //Walk
            if(footLanding && Mathf.Abs(angleWithCol) >= GroundWall_angleLimit)
            {
                float rotateAngle = angleWithCol > 0f ? -(90f - angleWithCol) : (90f + angleWithCol);
                rotateAngle = rotateAngle * 0.5f;   //不要讓OKA傾斜太多

                transform.GetChild(OkaID_Now).rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotateAngle);
            }
            //Stick
            else if (Mathf.Abs(angleWithCol) < GroundWall_angleLimit)
            {
                if (!isStickOnWall && Mathf.Abs(xInput) > 0f && frontTouchWall)
                {
                    isStickOnWall = true; PlayerStatus.isWallSticking = true;
                    animator[OkaID_Now].SetTrigger("wallStickIn");
                    animator[OkaID_Now].SetBool("wall_isSticking", true);
                }
                else if (isStickOnWall && (Mathf.Abs(xInput) == 0f))
                {
                    isStickOnWall = false; PlayerStatus.isWallSticking = false;
                    animator[OkaID_Now].SetTrigger("wallJumpOut");
                    animator[OkaID_Now].SetBool("wall_isSticking", false);

                    transform.GetChild(OkaID_Now).rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f);
                }

                if (isStickOnWall)
                {
                    transform.GetChild(OkaID_Now).rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angleWithCol);
                }
            }
        }
    }
    void OnCollisionExit2D(Collision2D collision)   
    {
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 13)
        {
            if (isStickOnWall)
            {
                isStickOnWall = false; PlayerStatus.isWallSticking = false;
                animator[OkaID_Now].SetTrigger("wallJumpOut");
                animator[OkaID_Now].SetBool("wall_isSticking", false);
            }

            transform.GetChild(OkaID_Now).rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f);
        }
    }
    #endregion ================↑collider相關↑================

    #region ================↓受傷復原相關↓================
    //---受傷(多載:同時or個別設定水量髒污的影響)---
    public void TakeDamage(int damage)
    {
        if (!PlayerStatus.canBeHurt) return;

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
        if (!PlayerStatus.canBeHurt) return;

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
        spriteRenderer[OkaID_Now].color = new Color(1f, 0.3962386f, 0.3726415f);

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
        PlayerStatus.canMove = false;

        yield return new WaitForSeconds(duration);

        PlayerStatus.canMove = true;
    }

    //---水中---
    public void InWater()
    {
        //---水中漂浮&冒泡泡---
        if (OkaID_Now == 0)
        {
            if (water_area.waveCrest - transform.position.y > 0.8f) rb2d.AddForce(Vector2.up * iceFloatForce);
            else if (water_area.waveCrest - transform.position.y > 0f) rb2d.AddForce(Vector2.up * iceFloatForce * (water_area.waveCrest - transform.position.y / 1f));

            if (isBubbling) { bubbleMaker.Stop(); isBubbling = false; }
        }
        else
        {
            float depth = water_area.waveCrest - transform.position.y;
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
