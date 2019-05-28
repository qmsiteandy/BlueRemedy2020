using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    [Header("基本參數")]
    [Range(0, 2)]
    public int Oka_ID;
    public float speedLimit = 8.0f;     //移動速度上限
    public float accelTime = 0.5f;       //加速時間
    public float jumpForce = 650.0f;    //跳躍力道
    [HideInInspector]
    public bool allCanDo = true;
    [HideInInspector]
    public bool canMove = true;
    [HideInInspector]
    public bool facingRight = true;    //是否面向右

    [Header("跳躍判斷")]
    public Transform footCheck;         //檢查踩踏地板的點
    public Transform frontCheck;
    public float checkRadius = 0.4f;    //檢查踩踏地板的判斷半徑
    private LayerMask whatIsGround;      //檢查踩踏地板的地板圖層
    private LayerMask whatIsPlatform;
    private LayerMask whatIsWall;
    [HideInInspector]
    public bool grounded = true;        //是否在地上
    private bool walled = false;
    private bool secondJumping = false;
    private bool pressingJump = false;

    [Header("被刺攻擊")]
    public int thronLayerID = 14;
    private float thronAttackTimer = 0f;
    private float thronAttackDelay = 1f;
    private int thronAttack = 2;

    [Header("長草設定")]
    public int grassLayerID = 15;
    private Transform parent_transform;
    private Rigidbody2D rb2d;          //儲存主角的Rigidbody2D原件
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public float xSpeed = 0f;
    private PlayerEnergy playerEnergy;

    [Header("機關設定")]
    public float holePassingSped=0.07f;
    private bool isPassing;
    private GameObject noticeUI;


    void Start()
    {
        parent_transform = GetComponentInParent<Transform>();
        //取得主角的Rigidbody2D原件
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerEnergy = transform.parent.GetComponent<PlayerEnergy>();

        whatIsGround = LayerMask.GetMask("Ground");
        whatIsPlatform = LayerMask.GetMask("Platform");
        whatIsWall = LayerMask.GetMask("Wall");

        //-----

        noticeUI = this.transform.GetChild(3).gameObject;
        noticeUI.SetActive(false);
    }

    void FixedUpdate()
    {
        if (allCanDo)
        {
            OnGround();
            if (canMove)
            {
                Move();
                Jump();
            }
        }
    }

    void OnGround()
    {
        //以半徑圓範圍偵測是否在地上，儲存到grounded
        grounded = Physics2D.OverlapCircle(footCheck.position, checkRadius, whatIsGround) || Physics2D.OverlapCircle(footCheck.position, checkRadius, whatIsPlatform);
        walled = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsWall);
        if (grounded || walled) secondJumping = false;
    }

    void Move()
    {

        xSpeed = Input.GetAxis("Horizontal") * speedLimit;
        if (Mathf.Abs(xSpeed) < 0.1f) xSpeed = 0f;
        rb2d.velocity = new Vector2(xSpeed, rb2d.velocity.y);

        animator.SetFloat("xSpeed", Mathf.Abs(rb2d.velocity.x));

        //偵測移動方向及是否需轉面
        if (xSpeed > 0 && !facingRight)
        {
            Flip();
        }
        else if (xSpeed < 0 && facingRight)
        {
            Flip();
        }
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
        if (Input.GetButton("Jump") && !pressingJump)
        {
            //在地上
            if (grounded)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
                rb2d.AddForce(Vector2.up * jumpForce);
                //設定為不在地上
                grounded = false;
                pressingJump = true;
            }
            else if (walled)
            {
                if (facingRight) { xSpeed = 0f; rb2d.AddForce(new Vector2(-jumpForce *2, jumpForce *1.2f)); }
                else rb2d.AddForce(new Vector2(jumpForce * 2, jumpForce * 1.2f));
               
                pressingJump = true;
            }
            else if (!secondJumping && Oka_ID==2)
            {
                if (rb2d.velocity.y < 0) rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
                rb2d.AddForce(Vector2.up * jumpForce);

                secondJumping = true;
                pressingJump = true;
            }
        }
        //當跳躍鍵放開且此時未著地
        else if (Input.GetButtonUp("Jump") && !grounded)
        {
            //呼叫JumpRelease函示
            JumpRelease();
        }

        if (Input.GetButtonUp("Jump")) pressingJump = false;
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
        if (collider.gameObject.layer == grassLayerID) collider.gameObject.GetComponent<GrassControl>().GrowGrass();
        else if (collider.gameObject.layer == thronLayerID) thronAttackTimer = 0f;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.layer == thronLayerID)
        {
            thronAttackTimer -= Time.deltaTime;
            if (thronAttackTimer < 0f)
            {
                TakeDamage(thronAttack); thronAttackTimer = thronAttackDelay; StartCoroutine(DamagedColor());

                if (facingRight) { rb2d.velocity = Vector3.zero; rb2d.AddForce(new Vector2(-jumpForce * 1.5f, jumpForce * 0.6f)); }
                else { rb2d.velocity = Vector3.zero; rb2d.AddForce(new Vector2(jumpForce * 1.5f, jumpForce * 0.6f)); }
            }
        }
        else if (collider.gameObject.tag == "Hole" && Oka_ID == 1)
        {
            if(!isPassing)noticeUI.SetActive(true);
            if (Input.GetButtonDown("Special") && !isPassing) PassHole(collider);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
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

    public void TakeDamage(int damage)
    {
        playerEnergy.ModifyDirt(damage);
        playerEnergy.ModifyWaterEnergy(-damage);
    }

    IEnumerator DamagedColor()
    {
        spriteRenderer.color = new Color(0.7f, 0f, 0f);

        yield return new WaitForSeconds(0.08f);

        spriteRenderer.color = new Color(1f, 1f, 1f);
    }

    IEnumerator HolePassing(bool isHorizontal , Vector3 start , Vector3 end)
    {
        allCanDo = false; isPassing = true;
        spriteRenderer.sortingLayerName = "Default";
        this.GetComponent<CircleCollider2D>().enabled = false;

        this.transform.position = start;

        if(isHorizontal) while(this.transform.position != end)
            {
                Vector3 newPos = Vector3.Lerp(this.transform.position, end, holePassingSped);
                newPos = new Vector3(newPos.x, end.y, newPos.z);
                this.transform.position = newPos;
                if (Mathf.Abs(this.transform.position.x - end.x) < 0.8f) this.transform.position = end;
                Debug.Log("this " + this.transform.position.x + " end.x " + end.x);

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
}
