using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_base : MonoBehaviour {

    GameObject target;
    Animator animator;
    SpriteRenderer spriteRenderer;  //怪物圖的render
    Vector2 centerPos;      //移動的區域中點
    Enemy_Dead enemy_dead;
    Enemy_Attack enemy_attack;
    GameObject enemy;
    public CameraControl cameraControl;

    [Header("Move Settings")]
    public float moveRange = 4f;    //移動範圍半徑
    private float posNow = 0f;      //目前移動相對中點的位置
    private bool goRight = true;    //是否往右走
    public float trackSpeed = 6f;
    public float closeRange = 1.25f;
    private Rigidbody2D rb2d;
    private CircleCollider2D BodyCollider;

    [Header("Track Settings")]
    public float FollowRadius = 4.5f;
    private Collider2D[] playerCol = { null };
    private ContactFilter2D playerFilter;

    [Header("Awake Settings")]
    private float bornTime = 8f;

    [Header("Action Settings")]
    public bool isTracking = false;
    public bool isAttacking;
    public bool isInjury;
    public bool isBorn;

    [Header("Mud Splash")]
    public GameObject mudSplashFX;
    public float enemyColRadius;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        BodyCollider = GetComponent<CircleCollider2D>();
        centerPos = transform.position; //移動中點設為最初的位置
    }

    void Start()
    {
        enemy = transform.GetChild(0).gameObject;
        enemy_attack = transform.GetComponentInChildren<Enemy_Attack>();
        enemy_dead = transform.GetComponent<Enemy_Dead>();
        rb2d = GetComponent<Rigidbody2D>();
        playerFilter.SetLayerMask(LayerMask.GetMask("Player"));
        cameraControl = GameObject.Find("CameraHolder").GetComponent<CameraControl>();

        enemyColRadius = GetComponent<CircleCollider2D>().radius;   //for setting mud splash position
    }

    void Update()
    {
        if(!enemy_dead.isDead){
            FindPlayer();

            if (!isTracking)
            {
                animator.SetBool("Walk", false);
            }
            else
            {
                if (enemy_attack.Attack_Wait == true)
                {
                    animator.SetBool("Walk", false);
                }
                else if(isTracking || enemy_attack.Attack_Wait== false)
                {
                    Tracking();
                }
            }
        }
    }

    #region ================↓追蹤主角↓================
    void FindPlayer()
    {
        bool traceMode = isTracking;

        isTracking = Physics2D.OverlapCircle(this.transform.position, FollowRadius, playerFilter.layerMask);

        if(traceMode == false && isTracking == true)
        {
            Physics2D.OverlapCircle(this.transform.position, FollowRadius, playerFilter, playerCol);
            target = playerCol[0].gameObject;
        }
    }

    void Tracking()
    {
        if (target != null && !isAttacking)
        {
           
            animator.SetBool("Walk", true);
            Vector3 diff = new Vector3(target.transform.position.x - transform.position.x, 0, 0);
            if (Mathf.Abs(diff.x) <= closeRange) return;

            if (diff.x > 0 && transform.position.x < centerPos.x + moveRange)
            {
                rb2d.velocity = new Vector2(trackSpeed, rb2d.velocity.y);
            }
            else if (diff.x < 0 && transform.position.x > centerPos.x - moveRange)
            {
                rb2d.velocity = new Vector2(-trackSpeed, rb2d.velocity.y);
            }

            float face = Mathf.Sign(diff.x);
            goRight = (face >= 0) ? true : false;
            Vector3 faceVec = new Vector3(face, 1, 1);
            transform.localScale = faceVec;
        }
    }
    #endregion ================↑追蹤主角↑================

    #region ================↓受到攻擊↓================
    public void TakeDamage(int damage)
    {
        if (!enemy_dead.isDead)
        {
            enemy_dead.health -= damage;
            if (enemy_dead.health <= 0)
            {
                enemy_dead.health = 0;
                enemy_dead.isDead = true;
                animator.SetTrigger("Dead");
            }
            isInjury = true;
            if(isAttacking == false)
            {
                animator.SetTrigger("Injury");
            }
            StartCoroutine(ChangeColor(new Color(1f, 0.3962386f, 0.3726415f), 0.1f));


            Vector3 mudSplashPosition = this.transform.position + new Vector3((PlayerControl.facingRight ? enemyColRadius * 0.5f : -enemyColRadius * 0.5f), 0f, 0f);
            Vector3 mudSplashRotation = new Vector3(0f, PlayerControl.facingRight ? 0f : 180f, 0f);
            GameObject FX_obj = Instantiate(mudSplashFX, mudSplashPosition,Quaternion.Euler(mudSplashRotation));
            Destroy(FX_obj, 1f);
        }
    }
    #endregion ================↑受到攻擊↑================

    public void InjuryOver()
    {
        isInjury = false;
    }

    public void AttackStart()
    {
        isAttacking = true;
        enemy_attack.Attack_Wait = false;
    }

    public void AttackOver()
    {
        isAttacking = false;
        enemy_attack.Attack_Wait = true;
    }

    public void Attack_Damage()
    {
        enemy_attack.Damage();
    }

    public void BodyColliderClose()
    {
        BodyCollider.offset = new Vector2(-0.02f, 2f);
        BodyCollider.radius = 0.01f;
        rb2d.isKinematic = true;
    }

    public void BodyColliderOpen()
    {
        rb2d.isKinematic = false;
        BodyCollider.offset = new Vector2(-0.02f, -0.65f);
        BodyCollider.radius = 1.169405f;
    }

    //public void CameraShake()
    //{
    //    cameraControl.Shake(0.3f, 0.1f, 0.05f);
    //}

    IEnumerator ChangeColor(Color color, float colorChangeTime)
    {
        spriteRenderer.color = color;

        yield return new WaitForSeconds(colorChangeTime);

        spriteRenderer.color = new Color(1, 1, 1);
    }

}


