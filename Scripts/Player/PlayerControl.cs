using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    [Header("基本參數")]
    [Range(0, 2)]static public int OkaID_Now = 1;
    public float xInput;
    public float initSpeedLimit = 8f;
    [HideInInspector] static public float speedLimit;           //移動速度上限
    private float jumpForce = 700.0f;    //跳躍力道
    private float walljumpForce = 750.0f;
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
    private float checkRadius = 0.2f;    //檢查踩踏地板的判斷半徑
    private LayerMask whatIsGround;      //檢查踩踏地板的地板圖層
    private LayerMask whatIsPlatform;
    [HideInInspector] static public bool footLanding = false;
    public bool onGround, onPlatform;
    public GameObject objUnderFoot;
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
    public NoticeUIControl noticeUIControl;
    public CameraControl cameraControl;


    [Header("水中")]
    //---"水基本"
    public bool isInWater = false;
    private Water_Area water_area;
    public GameObject waterSplashFX;
    //---"水對主角影響"
    private float waterDrag = 2.0f;  //水對主角阻力
    private float speedDownRate = 0.8f;  //水導致主角降速比例
    //---"泡泡"
    public ParticleSystem bubbleMaker;
    private bool isBubbling = false;
    private float iceFloatForce = 50f;

    [Header("髒污ripple")]
    private SpriteMask rippleMask;

    void Awake()
    {
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
        noticeUIControl = this.transform.Find("noticeUI_folder").GetComponent<NoticeUIControl>();
        
        //NoticeUI_Setting(999);

        //---bubble maker init---
        bubbleMaker = transform.Find("BubbleMaker").GetComponent<ParticleSystem>();
        bubbleMaker.Stop();

        //---用於髒污ripple的材質遮罩設定---
        rippleMask = this.GetComponent<SpriteMask>();
    }

    void Start()
    {
        for (int x = 0; x < 3; x++) { if (transform.GetChild(x).gameObject.active == true) OkaID_Now = x; }
        speedLimit = initSpeedLimit;
        facingRight = true;
        noticeUIControl.NoticeUI_Setting(999);
    }

    void FixedUpdate()
    {
        //if(!isInWater) PointCheck();
        PointCheck();

        Move();
        Jump();
    }

    void Update()
    {
        //---用於髒污塗層的遮罩---
        rippleMask.sprite = spriteRenderer[OkaID_Now].sprite;

        //Sleep
        if (PlayerStatus.isSleeping)
        {
            if(Input.anyKeyDown) animator[OkaID_Now].SetTrigger("sleepAwake");
        }

        //InWater
        if (isInWater) InWater();

    }

    void PointCheck()
    {
        //以半徑圓範圍偵測是否在地上，儲存到grounded
        if(onGround = Physics2D.OverlapCircle(footCheck.position, checkRadius, whatIsGround)) objUnderFoot = Physics2D.OverlapCircle(footCheck.position, checkRadius, whatIsGround).gameObject;
        if(onPlatform = Physics2D.OverlapCircle(footCheck.position, checkRadius, whatIsPlatform)) objUnderFoot = Physics2D.OverlapCircle(footCheck.position, checkRadius, whatIsPlatform).gameObject;

        footLanding = onGround || onPlatform || isInWater;

        frontTouchWall = Physics2D.OverlapCircle(frontCheck.position, 0.45f, whatIsGround);
        backTouchWall = Physics2D.OverlapCircle(backCheck.position, 0.35f, whatIsGround);

        touchGround = (footLanding || frontTouchWall || backTouchWall);

        if (footLanding || frontTouchWall || backTouchWall) secondJumping = false; 
    }

    #region ================↓移動轉身相關↓================
    void Move()
    {
        xInput = Input.GetAxis("Horizontal") + Input.GetAxis("XBOX_Horizontal");

        
        if (PlayerStatus.canMove)
        {
            rb2d.velocity = new Vector2(speedLimit * xInput, rb2d.velocity.y);
        }
        else
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x * 0.85f, rb2d.velocity.y);
        }

        if (frontTouchWall)
        {
            if (Mathf.Abs(rb2d.velocity.x) > 0.4f) rb2d.velocity = new Vector3(rb2d.velocity.x * 0.9f, rb2d.velocity.y, 0f);
            else rb2d.velocity = new Vector3(0f, rb2d.velocity.y, 0f);
        }

        animator[OkaID_Now].SetFloat("xSpeed", Mathf.Abs(rb2d.velocity.x));

        if (xInput != 0f && PlayerStatus.canFlip)
        {
            if ((facingRight && xInput < 0f) || (!facingRight && xInput > 0f)) Flip();
        }

        xSpeed = rb2d.velocity.x;
    }
    void Flip() //偵測移動方向及是否需轉面
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    #endregion ================↑移動轉身相關↑================

    #region ================↓跳躍相關↓================
    void Jump()
    {
        animator[OkaID_Now].SetBool("touchGroung", touchGround);
        jumping = !touchGround;

        animator[OkaID_Now].SetBool("jumpFall", false);
        animator[OkaID_Now].SetBool("jumpUp", false);

        if (jumping)
        {
            if(rb2d.velocity.y < -0.1f) animator[OkaID_Now].SetBool("jumpFall", true);
            if (rb2d.velocity.y > 0.1f) animator[OkaID_Now].SetBool("jumpUp", true);
        }

        if (!PlayerStatus.canJump) return;

        //平台上往下跳
        if (onPlatform && (Input.GetAxis("Vertical") < 0f || Input.GetAxis("XBOX_Vertical") < -0.5f) && !jumping)
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
        float colDisTime = 0.5f;

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
        //---SceneTrigger
        if (collider.gameObject.layer == LayerMask.NameToLayer("SceneTrigger"))
        {
            PlayerStatus.isInInteractTrigger = true;
        }
        //---水中---
        if (collider.gameObject.layer == LayerMask.NameToLayer("WaterArea"))
        {
            water_area = collider.GetComponent<Water_Area>();

            for (int x = 0; x < 3; x++) { spriteRenderer[x].sortingLayerName = "Scene"; spriteRenderer[x].sortingOrder = -1; }
            speedLimit *= speedDownRate;
            rb2d.drag = waterDrag;
            isInWater = true;

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
        if (collider.gameObject.tag == "WaterPassingTrigger")
        {
            //---滲透毛細提示UI&起始呼叫---
            if (OkaID_Now == 1 && !skill_Water.isPassing)
            {
                if (collider.name == "root_trigger" ||
                    ((collider.name == "firstEnd" && facingRight) || (collider.name == "secondEnd" && !facingRight)))
                {
                    noticeUIControl.NoticeUI_Setting(3);    //跳出驚嘆號
                        
                    skill_Water.WaitPassInput(collider);
                }
                else noticeUIControl.NoticeUI_Setting(999);
            }
            else if (OkaID_Now == 0 || OkaID_Now == 2) noticeUIControl.NoticeUI_Setting(1); //跳出water提示
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        //---SceneTrigger
        if (collider.gameObject.layer == LayerMask.NameToLayer("SceneTrigger"))
        {
            PlayerStatus.isInInteractTrigger = false;
        }
        //---水中---
        if (collider.gameObject.layer == LayerMask.NameToLayer("WaterArea"))
        {
            for (int x = 0; x < 3; x++) { spriteRenderer[x].sortingLayerName = "Player"; spriteRenderer[x].sortingOrder = 0; }
            speedLimit = initSpeedLimit;
            rb2d.drag = 0f;
            isInWater = false;


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
        if (collider.gameObject.tag == "WaterPassingTrigger")
        {
            noticeUIControl.NoticeUI_Setting(999);  //關閉提示UI
            PlayerStatus.isInInteractTrigger = false;
        }
    }
    #endregion ================↑trigger相關↑================

    #region ================↓collider相關↓================
    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground & Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            Vector2 contactNormal = collision.GetContact(0).normal; //取得交點法向量  
            angleWithCol = (Mathf.Atan(contactNormal.y / contactNormal.x)) * 180f / Mathf.PI; //計算角度

            ////Walk
            //if(footLanding && Mathf.Abs(angleWithCol) >= GroundWall_angleLimit)
            //{
            //    float rotateAngle = angleWithCol > 0f ? -(90f - angleWithCol) : (90f + angleWithCol);
            //    rotateAngle = rotateAngle * 0.5f;   //不要讓OKA傾斜太多

            //    transform.GetChild(OkaID_Now).rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotateAngle);
            //}

            //Stick
            if (Mathf.Abs(angleWithCol) < GroundWall_angleLimit && !footLanding)
            {
                if (!isStickOnWall && Mathf.Abs(xInput) > 0f && frontTouchWall)
                {
                    isStickOnWall = true; PlayerStatus.isWallSticking = true;
                    animator[OkaID_Now].SetTrigger("wallStickIn");
                    animator[OkaID_Now].SetBool("wall_isSticking", true);
                }
                else if (isStickOnWall && ((Mathf.Abs(xInput) == 0f) || !frontTouchWall))
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground & Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
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

        //if (skill_Water.isPassing) skill_Water.PassingInterrupted();
        skill_Base[OkaID_Now].BackIdle();

        playerEnergy.ModifyDirt(damage);
        playerEnergy.ModifyWaterEnergy(-damage);

        animator[OkaID_Now].SetTrigger("gotHurt");
        StartCoroutine(ShortCantMove(0.2f));

        StartCoroutine(DamageHitRecover());
    }
    public void TakeDamage(int waterCost,int addDirt)
    {
        if (!PlayerStatus.canBeHurt) return;

        //if (skill_Water.isPassing) skill_Water.PassingInterrupted();
        skill_Base[OkaID_Now].BackIdle();

        playerEnergy.ModifyDirt(addDirt);
        playerEnergy.ModifyWaterEnergy(- waterCost);

        animator[OkaID_Now].SetTrigger("gotHurt");
        StartCoroutine(ShortCantMove(0.2f));

        StartCoroutine(DamageHitRecover());
    }
    IEnumerator DamageHitRecover()
    {
        spriteRenderer[OkaID_Now].color = new Color(1f, 0.3962386f, 0.3726415f);
        PlayerStatus.isHitRecover = true;

        yield return new WaitForSeconds(0.08f);

        spriteRenderer[OkaID_Now].color = new Color(1f, 1f, 1f);
        PlayerStatus.isHitRecover = false;
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


    public void FallAsleep()
    {
        PlayerStatus.isSleeping = true;
        animator[OkaID_Now].SetTrigger("fallAsleep");
    }
    public void SleepAwake()   //sleepAwake animation呼叫skillBase，再由skillbase呼叫此函式
    {
        PlayerStatus.isSleeping = false;
    }
}
