using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 250.0f;    //移動速度
    public float speedLimit = 4.0f;     //移動速度上限
    public float jumpForce = 350.0f;    //跳躍力道
    public int healthMax = 3000;
    private int health;

    public Transform footCheck;         //檢查踩踏地板的點
    public float checkRadius = 0.2f;    //檢查踩踏地板的判斷半徑
    public LayerMask whatIsGround;      //檢查踩踏地板的地板圖層
    public bool grounded = true;        //是否在地上
    public bool canMove = true;         //是否可移動
    public bool allCanDo = true;
    public bool isCalling = false;

    public FruitManager fruitManager;
    public CameraControl cameraControl;
    public UIHealth UI_health;

    public bool facingRight = true;    //是否面向右
    private bool canLookingUpOrDown = true;
    private Rigidbody2D rb2d;           //儲存主角的Rigidbody2D原件


    void Start()
    {
        //取得主角的Rigidbody2D原件
        rb2d = GetComponent<Rigidbody2D>();

        health = healthMax;
    }

    //規律的Update，主角才要
    void FixedUpdate()
    {
        if (allCanDo)
        {
            OnGround();
            Move();
            Jump();
            LookUpDown();
        }
    }

    void Update()
    {
        //測試用
        if (Input.GetKeyDown(KeyCode.B)) TakeDamage(300);
        if (Input.GetKeyDown(KeyCode.N)) Heal(500);

        if (health <= 0)
        {
            Die();
        }

        if (Input.GetButtonDown("Call")) isCalling = true;
        else if (Input.GetButtonUp("Call")) isCalling = false;
    }

    //檢查是否在地面
    void OnGround()
    {
        //以半徑圓範圍偵測是否在地上，儲存到grounded
        grounded = Physics2D.OverlapCircle(footCheck.position, checkRadius, whatIsGround);
    }


    void Move()
    {
        if (canMove)
        {
            //水平方向輸入並乘上移動速度
            float xSpeed = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            //限制xSpeed在速度限制範圍內
            xSpeed = Mathf.Clamp(xSpeed, -speedLimit, speedLimit);
            //設定主角水平速度
            rb2d.velocity = new Vector2(xSpeed, rb2d.velocity.y);

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
    }

    //轉面
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

    //跳躍
    void Jump()
    {
        //跳躍鍵按著
        if (Input.GetButton("Jump") && canMove)
        {
            //在地上
            if (grounded)
            {
                //給予主角彈跳力道
                rb2d.AddForce(Vector2.up * jumpForce);
                //設定為不在地上
                grounded = false;
            }
        }
        //當跳躍鍵放開且此時未著地
        else if (Input.GetButtonUp("Jump") && !grounded)
        {
            //呼叫JumpRelease函示
            JumpRelease();
        }
    }

    //彈跳釋放(會影響長按短按地跳躍高度)
    void JumpRelease()
    {
        //若主角已經是下降狀態就直接離開函式
        if (rb2d.velocity.y <= 0) return;
        //設定主角的垂直速度打3折
        rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.3f);
    }


    void LookUpDown()
    {
        float yInput = Input.GetAxis("Vertical");

        if (rb2d.velocity.x == 0)
        {
            if (yInput > 0 && canLookingUpOrDown && grounded)
            {
                canLookingUpOrDown = false;

                if (isCalling) return;

                cameraControl.PlayerLookUp();
                canMove = false;
            }
            else if (yInput < 0 && canLookingUpOrDown && grounded)
            {
                canLookingUpOrDown = false;

                if (isCalling) return;

                cameraControl.PlayerLookDown();
                canMove = false;
            }
        }
        if (yInput == 0 && !isCalling)
        {
            cameraControl.BackNormal();
            canLookingUpOrDown = true;
            canMove = true;
        }
    }

    void Die()
    {

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;

        UI_health.SetHealthUI(health);
        fruitManager.FirstLoseFress(damage);
    }

    public void Heal(int heal)
    {
        health += heal;
        if (health > healthMax) health = healthMax;

        UI_health.SetHealthUI(health);
    }
}
