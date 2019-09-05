using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    [Header("基本參數")]
    [Range(0, 2)] public int Oka_ID;
    public float initSpeedLimit = 8.0f;
    [HideInInspector] public float speedLimit;           //移動速度上限
    public float accelTime = 0.5f;       //加速時間
    [HideInInspector] public float xSpeed = 0f;
    public float jumpForce = 650.0f;    //跳躍力道
    [HideInInspector] public Rigidbody2D rb2d;          //儲存主角的Rigidbody2D原件
    [HideInInspector] public bool allCanDo = true;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool facingRight = true;    //是否面向右
    private SpriteRenderer spriteRenderer;  //用來設定sorting
    private Animator animator;
    private PlayerEnergy playerEnergy;

    [Header("跳躍判斷")]
    public Transform footCheck;         //檢查踩踏地板的點
    public Transform frontCheck, backCheck;
    public float checkRadius = 0.4f;    //檢查踩踏地板的判斷半徑
    private LayerMask whatIsGround;      //檢查踩踏地板的地板圖層
    private LayerMask whatIsPlatform;
    private LayerMask whatIsWall;
    [HideInInspector] public bool jumpable = false;
    private bool onGround, onPlatform;
    private bool frontTouchWall = false, backTouchWall = false;
    private bool secondJumping = false;
    private bool pressingJump = false;
    private bool jumpingDown = false;
    public float shortCantMoveDuration = 0.08f;

    [Header("長草設定")]
    public int grassLayerID = 15;
    
    [Header("機關設定")]
    public float holePassingSped=0.07f;
    private bool isPassing;
    public GameObject noticeUI;

    [Header("水中")]
    public Transform bubbleMakerTran;
    private ParticleSystem bubbleMaker;
    private bool isBubbling = false;
    private int DirtyWaterLayerID = 14;
    private float iceFloatForce = 50f;
    public bool isInWater = false;
    private DirtyWater dirtyWater;
    


    void Start()
    {
        speedLimit = initSpeedLimit;

        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerEnergy = transform.parent.GetComponent<PlayerEnergy>();

        bubbleMaker = bubbleMakerTran.GetComponentInParent<ParticleSystem>();


        //---各種圖層MASK設定---
        whatIsGround = LayerMask.GetMask("Ground");
        whatIsPlatform = LayerMask.GetMask("Platform");
        whatIsWall = LayerMask.GetMask("Wall");

        //---NoticeMark--
        noticeUI.SetActive(false);

        //---bubble maker init---
        bubbleMaker.Stop();
    }

    void FixedUpdate()
    {
        if (allCanDo)
        {
            if(!isInWater) PointCheck();

            if (canMove)
            {
                Move();
                Jump();
            }
        }
    }

    void PointCheck()
    {
        //以半徑圓範圍偵測是否在地上，儲存到grounded
        onGround = Physics2D.OverlapCircle(footCheck.position, checkRadius, whatIsGround);
        onPlatform = Physics2D.OverlapCircle(footCheck.position, checkRadius, whatIsPlatform);
        jumpable = onGround || onPlatform;
        frontTouchWall = Physics2D.OverlapCircle(frontCheck.position, 0.1f, whatIsWall);
        backTouchWall = Physics2D.OverlapCircle(backCheck.position, 0.1f, whatIsWall);
        if (jumpable || frontTouchWall || backTouchWall) secondJumping = false;
    }

    void Move()
    {
        /*xSpeed = Mathf.Lerp(xSpeed, Input.GetAxis("Horizontal") * speedLimit, 0.5f);
        if (Mathf.Abs(xSpeed) < 0.1f) xSpeed = 0f;
        animator.SetFloat("xSpeed", Mathf.Abs(rb2d.velocity.x));

        if (frontTouchWall)
        {
            xSpeed *= 0.3f;
            animator.SetFloat("xSpeed", 0f);
        }

        rb2d.velocity = new Vector2(xSpeed, rb2d.velocity.y);*/

        float xInput = Input.GetAxis("Horizontal");

        Vector2 force = new Vector2(xInput * 50, 0f);
        rb2d.AddForce(force);

        if (xInput != 0) rb2d.velocity = new Vector3(Mathf.Clamp(rb2d.velocity.x, xInput * speedLimit, xInput * speedLimit), rb2d.velocity.y, 0f);
        else rb2d.velocity = new Vector3(0f, rb2d.velocity.y, 0f);

        animator.SetFloat("xSpeed", rb2d.velocity.x);

        #region flipping()
        //偵測移動方向及是否需轉面
        if (xInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (xInput < 0 && facingRight)
        {
            Flip();
        }
        #endregion
    }

    void Flip()
    {
        //bool變數代表方向
        facingRight = !facingRight;
        //取得目前物件的規格
        Vector3 theScale = transform.localScale;
        //使規格的水平方向相反
        theScale.x *= -1;
        //套用翻面後的規格
        transform.localScale = theScale;
    }

    void Jump()
    {
        //平台上往下跳
        if (onPlatform && Input.GetAxis("Vertical") < -0.1f && !jumpingDown)
        {
            StartCoroutine(JumpDownFromPlat());
            jumpingDown = true;
            return;
        }
        if (Input.GetAxis("Jump") > 0.1f && !pressingJump) 
        {
            pressingJump = true;
                        
            //在地上
            if (jumpable)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);

                if (!isInWater) rb2d.AddForce(Vector2.up * jumpForce);
                else rb2d.AddForce(Vector2.up * jumpForce * 0.8f);
            }
            //蹬牆跳
            else if (frontTouchWall)
            {
                if (facingRight) { rb2d.AddForce(new Vector2(-jumpForce, jumpForce)); }
                else rb2d.AddForce(new Vector2(jumpForce, jumpForce));

                StartCoroutine(ShortCantMove(shortCantMoveDuration));
            }
            else if (backTouchWall)
            {
                if (facingRight) { rb2d.AddForce(new Vector2(jumpForce, jumpForce)); }
                else rb2d.AddForce(new Vector2(-jumpForce, jumpForce));

                StartCoroutine(ShortCantMove(shortCantMoveDuration));
            }
            //二段跳
            else if (!secondJumping && Oka_ID==2)
            {
                if (rb2d.velocity.y < 0) rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);

                if (!isInWater) rb2d.AddForce(Vector2.up * jumpForce);
                else rb2d.AddForce(Vector2.up * jumpForce * 0.8f);

                secondJumping = true;               
            }
        }
        //當跳躍鍵放開且此時未著地
        else if (Input.GetAxis("Jump") < 1f && !jumpable)
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

    //---------------------------------------------

    void OnTriggerEnter2D(Collider2D collider)
    {
        //---長草---
        if (collider.gameObject.layer == grassLayerID) collider.gameObject.GetComponent<GrassControl>().GrowGrass();

        //---水中---
        if (collider.gameObject.layer == DirtyWaterLayerID)
        {
            dirtyWater = collider.GetComponent<DirtyWater>();
        }
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        //---穿洞---
        if (collider.gameObject.tag == "Hole" && Oka_ID == 1)
        {
            if (!isPassing) noticeUI.SetActive(true);
            if (Input.GetButtonDown("Special") && !isPassing) PassHole(collider);
        }

    }
    void OnTriggerExit2D(Collider2D collider)
    {
        //---穿洞---
        if (collider.gameObject.tag == "Hole" && Oka_ID == 1) noticeUI.SetActive(false);

        
    }

    //---------------------------------------------

    void PassHole(Collider2D holeCollider) 
    {
        noticeUI.SetActive(false);

        BoxCollider2D holeCol = holeCollider.GetComponent<BoxCollider2D>();

        bool isHorizontal = holeCol.size.y / holeCol.size.x < 1 ? true : false;

        Vector3 start, end;
        if (isHorizontal)
        {
            if(transform.position.x < holeCollider.transform.position.x)
            {
                start = new Vector3(this.transform.position.x, holeCollider.transform.position.y, holeCollider.transform.position.z); end = holeCollider.transform.position + new Vector3(holeCol.size.x / 2, 0f, 0f);
            }
            else
            {
                start = new Vector3(this.transform.position.x, holeCollider.transform.position.y, holeCollider.transform.position.z); end = holeCollider.transform.position - new Vector3(holeCol.size.x / 2, 0f, 0f);
            }
        }
        else
        {
            if (transform.position.y < holeCollider.transform.position.y)
            {
                start = new Vector3(holeCollider.transform.position.x, this.transform.position.y, holeCollider.transform.position.z); end = holeCollider.transform.position + new Vector3(0f, holeCol.size.y / 2, 0f);
            }
            else
            {
                start = new Vector3(holeCollider.transform.position.x, this.transform.position.y, holeCollider.transform.position.z); end = holeCollider.transform.position - new Vector3(0f, holeCol.size.y / 2, 0f);
            }
        }
        StartCoroutine(HolePassing(isHorizontal, start, end));
    }

    IEnumerator HolePassing(bool isHorizontal, Vector3 start, Vector3 end)
    {
        allCanDo = false; isPassing = true;
        spriteRenderer.sortingLayerName = "Default";
        this.GetComponent<CircleCollider2D>().enabled = false;

        this.transform.position = start;

        if (isHorizontal) while (this.transform.position != end)
            {
                Vector3 newPos = Vector3.Lerp(this.transform.position, end, holePassingSped);
                newPos = new Vector3(newPos.x, end.y, newPos.z);
                this.transform.position = newPos;
                if (Mathf.Abs(this.transform.position.x - end.x) < 0.8f) this.transform.position = end;

                yield return null;
            }
        else while (this.transform.position != end)
            {
                Vector3 newPos = Vector3.Lerp(this.transform.position, end, holePassingSped);
                newPos = new Vector3(end.x, newPos.y, newPos.z);
                this.transform.position = newPos;
                if (Mathf.Abs(this.transform.position.y - end.y) < 0.8f) this.transform.position = end;

                yield return null;
            }

        allCanDo = true; isPassing = false;
        spriteRenderer.sortingLayerName = "Player";
        this.GetComponent<CircleCollider2D>().enabled = true;
    }

    public void TakeDamage(int damage)
    {
        playerEnergy.ModifyDirt(damage);
        playerEnergy.ModifyWaterEnergy(-damage);

        StartCoroutine(DamagedColor());
    }
    public void TakeDamage(int waterCost,int addDirt)
    {
        playerEnergy.ModifyDirt(addDirt);
        playerEnergy.ModifyWaterEnergy(- waterCost);

        StartCoroutine(DamagedColor());
    }

    IEnumerator DamagedColor()
    {
        spriteRenderer.color = new Color(0.7f, 0f, 0f);

        yield return new WaitForSeconds(0.08f);

        spriteRenderer.color = new Color(1f, 1f, 1f);
    }

    public void TakeHeal(int waterHeal, int dirtHeal)
    {
        playerEnergy.ModifyWaterEnergy(waterHeal);
        playerEnergy.ModifyDirt(-dirtHeal);
    }

    IEnumerator JumpDownFromPlat()
    {
        float timer = 0f;
        float colDisTime = 0.2f;

        this.GetComponent<CircleCollider2D>().enabled = false;
        while (timer < colDisTime)
        {
            timer += Time.deltaTime;
            yield return 0;
        }
        this.GetComponent<CircleCollider2D>().enabled = true;

        jumpingDown = false;
    }

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

    public void InWater()
    {
        //---水中漂浮&冒泡泡---
        
        jumpable = true;

        if (Oka_ID == 0)
        {
            if (dirtyWater.waveCrest - transform.position.y > 0.8f) rb2d.AddForce(Vector2.up * iceFloatForce);
            else if (dirtyWater.waveCrest - transform.position.y > 0f) rb2d.AddForce(Vector2.up * iceFloatForce * (dirtyWater.waveCrest - transform.position.y / 1f));
        }
        else
        {
            bubbleMakerTran.position = this.transform.position;

            float depth = dirtyWater.waveCrest - transform.position.y;
            if (depth > 0.5f) bubbleMaker.startLifetime = depth * 0.8f;

            if (depth > 0.5f && !isBubbling) { bubbleMaker.Play(); isBubbling = true; }
            else if (depth < 0.5f && isBubbling) { bubbleMaker.Stop(); isBubbling = false; }
        }
        
    }
}
