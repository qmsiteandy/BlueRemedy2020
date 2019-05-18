using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    [Header("基本參數")]
    public float speedLimit = 8.0f;     //移動速度上限
    public float accelTime = 0.5f;       //加速時間
    public float jumpForce = 650.0f;    //跳躍力道

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

    [HideInInspector]
    public bool allCanDo = true;
    [HideInInspector]
    public bool canMove = true;
    [HideInInspector]
    public bool facingRight = true;    //是否面向右

    private Transform parent_transform;
    private Rigidbody2D rb2d;          //儲存主角的Rigidbody2D原件
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public float xSpeed = 0f;

    void Start()
    {
        parent_transform = GetComponentInParent<Transform>();
        //取得主角的Rigidbody2D原件
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        whatIsGround = LayerMask.GetMask("Ground");
        whatIsPlatform = LayerMask.GetMask("Platform");
        whatIsWall = LayerMask.GetMask("Wall");
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

        xSpeed = Mathf.Lerp(xSpeed, Input.GetAxis("Horizontal") * speedLimit, accelTime);
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
            else if (!secondJumping)
            {
                if (rb2d.velocity.y < 0) rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.3f);
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

    IEnumerator DamagedColor()
    {
        spriteRenderer.color = new Color(0.7f, 0f, 0f);

        yield return new WaitForSeconds(0.08f);

        spriteRenderer.color = new Color(1f, 1f, 1f);
    }
}
