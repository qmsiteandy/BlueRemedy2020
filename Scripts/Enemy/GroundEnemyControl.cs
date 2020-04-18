using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyControl : Enemy_base {

    [Header("Move Settings")]
    public Vector2 centerPos;      //移動的區域中點
    public float moveRange = 4f;    //移動範圍半徑
    private float posNow = 0f;      //目前移動相對中點的位置
    private bool goRight = true;    //是否往右走
    public float trackSpeed = 6f;
    public float closeRange = 1.25f;
    private CircleCollider2D BodyCollider;
    public Vector2 BodyColliderOffset;

    [Header("Track Settings")]
    GameObject target;
    public float FollowRadius = 4.5f;
    private LayerMask playerFilter;
    public bool isTracking = false;

    [Header("Awake Settings")]
    private float bornTime = 8f;

    [Header("Mud Splash")]
    public GameObject mudSplashFX;
    public float enemyColRadius;

    // Use this for initialization
    void Awake()
    {
        BaseAwake();    //父腳本的Awake
 
        centerPos = transform.position; //移動中點設為最初的位置

        playerFilter = LayerMask.GetMask("Player");

        BodyCollider = GetComponent<CircleCollider2D>();
        BodyColliderOffset = BodyCollider.offset;
        enemyColRadius = GetComponent<CircleCollider2D>().radius;   //for setting mud splash position
    }

    void Update()
    {
        if (!enemy_dead.isDead)
        {
            FindPlayer();

            if (!isTracking || isFreeze == true)
            {
                animator.SetBool("Walk", false);
            }
            else
            {
                if (enemy_attack.Attack_Wait == true)
                {
                    animator.SetBool("Walk", false);
                }
                else if (isTracking || enemy_attack.Attack_Wait == false)
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

        isTracking = Physics2D.OverlapCircle(this.transform.position, FollowRadius, playerFilter);

        if (traceMode == false && isTracking == true)
        {
            Collider2D playerCol = Physics2D.OverlapCircle(this.transform.position, FollowRadius, playerFilter);
            target = playerCol.gameObject;
        }
    }

    void Tracking()
    {
        if (target != null && !isAttacking && isFreeze == false)
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

    //public void InjuryOver()
    //{
    //    isInjury = false;
    //}

    //public void CameraShake()
    //{
    //    cameraControl.Shake(0.3f, 0.1f, 0.05f);
    //}
    
    //覆寫TakeDamage
    public override void TakeDamage(int damage)
    {
        if (!enemy_dead.isDead)
        {
            enemy_dead.health -= damage;
            //isInjury = true;
            if (enemy_dead.health <= 0)
            {
                enemy_dead.health = 0;
                enemy_dead.isDead = true;
                animator.SetTrigger("Dead");
                isAttacking = false;
            }
            else if (isAttacking == false)
            {
                animator.SetTrigger("Injury");
                StartCoroutine(Freeze(0.5f));
            }
            StartCoroutine(ChangeColor(new Color(1f, 0.3962386f, 0.3726415f), 0.1f));


            //Vector3 mudSplashPosition = this.transform.position + new Vector3((PlayerControl.facingRight ? enemyColRadius * 0.5f : -enemyColRadius * 0.5f), 0f, 0f);
            //Vector3 mudSplashRotation = new Vector3(0f, PlayerControl.facingRight ? 0f : 180f, 0f);
            //GameObject FX_obj = Instantiate(mudSplashFX, mudSplashPosition, Quaternion.Euler(mudSplashRotation));
            //Destroy(FX_obj, 1f);
        }
    }

    public void BodyColliderClose()
    {
        BodyCollider.offset = new Vector2(-0.02f, 2f);
        BodyCollider.radius = 0.01f;
        //rb2d.isKinematic = true;
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void BodyColliderOpen()
    {
        rb2d.isKinematic = false;
        //BodyCollider.offset = new Vector2(-0.02f, -0.65f);
        BodyCollider.offset = BodyColliderOffset;
        BodyCollider.radius = 1.169405f;

        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }


}
