using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_base_ground : MonoBehaviour {

    GameObject target;
    Animator animator;
    SpriteRenderer spriteRenderer;  //怪物圖的render
    Enemy_Attack enemy_attack;
    Enemy_Dead enemy_dead;
    Vector2 centerPos;      //移動的區域中點
    GameObject enemy;

    [Header("Move Settings")]
    public float moveRange = 3.5f;    //移動範圍半徑
    public float moveSpeed = 1.5f;  //移動速度
    private float posNow = 0f;      //目前移動相對中點的位置
    private bool goRight = true;    //是否往右走
    public float trackSpeed = 1.5f;
    public float closeRange = 1.25f;

    [Header("Track Settings")]
    public float FollowRadius = 4.5f;
    private Collider2D[] playerCol = { null };
    private ContactFilter2D playerFilter;

    [Header("Awake Settings")]
    public float bornTime = 8f;

    [Header("Action Settings")]
    public bool isTracking = false;
    public bool isAttacking;
    public bool isInjury;
    public bool isBorn;


    // Use this for initialization
    void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        centerPos = transform.position; //移動中點設為最初的位置
    }

    void Start()
    {
        enemy = transform.GetChild(0).gameObject;
        enemy_dead = transform.GetComponent<Enemy_Dead>();
        enemy_attack = transform.GetComponentInChildren<Enemy_Attack>();
        playerFilter.SetLayerMask(LayerMask.GetMask("Player"));
    }

    void Update()
    {
        if (!enemy_dead.isDead)
        {
            FindPlayer();

            if (isTracking)
            {
                Tracking();
            }
            else
            {
                MoveAround();
            }
        }

    }


    #region ================↓來回移動↓================
    void MoveAround()
    {
        if (!isAttacking)
        {
            if (posNow >= moveRange || posNow <= -moveRange)
            {
                posNow = Mathf.Clamp(posNow, -moveRange, moveRange); //設定posNow到範圍邊界
                goRight = !goRight; //往回走
                if (goRight)
                {
                    enemy.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    enemy.transform.localScale = new Vector3(-1, 1, 1);
                }
            }

            if (goRight) posNow += moveSpeed * Time.deltaTime;
            else if (!goRight) posNow -= moveSpeed * Time.deltaTime;

            transform.position = new Vector3(centerPos.x + posNow, centerPos.y, 0f); //設定腳色的位置
        }
    }
    #endregion ================↑來回移動↑================

    #region ================↓追蹤主角↓================
    void FindPlayer()
    {
        bool traceMode = isTracking;

        isTracking = Physics2D.OverlapCircle(this.transform.position, FollowRadius, playerFilter.layerMask);

        if (traceMode == false && isTracking == true)
        {
            Physics2D.OverlapCircle(this.transform.position, FollowRadius, playerFilter, playerCol);
            target = playerCol[0].gameObject;
        }
    }

    void Tracking()
    {
        if (target != null && !isAttacking)
        {

            Vector3 diff = new Vector3(target.transform.position.x - transform.position.x, 0, 0);
            if (Mathf.Abs(diff.x) <= closeRange) return;
           
            posNow = Mathf.Lerp(posNow, target.transform.position.x - centerPos.x, trackSpeed * Time.deltaTime);
            posNow = Mathf.Clamp(posNow, -moveRange, moveRange);
            transform.position = new Vector3(centerPos.x + posNow, centerPos.y, 0f);

            float face = Mathf.Sign(diff.x);
            goRight = (face >= 0) ? true : false;
            Vector3 faceVec = new Vector3(face, 1, 1);
            enemy.transform.localScale = faceVec;
        }

    }
    #endregion ================↑追蹤主角↑================

    #region ================↓受到攻擊↓================
    public void TakeDamage(int damage)
    {
        if (!enemy_dead.isDead && isAttacking == false)
        {
            enemy_dead.health -= damage;
            if (enemy_dead.health <= 0)
            {
                enemy_dead.health = 0;
                enemy_dead.isDead = true;
                //animator.SetTrigger("Dead");
            }
            isInjury = true;
            //animator.SetTrigger("Injury");
            StartCoroutine(ChangeColor(new Color(1f, 0.3962386f, 0.3726415f), 0.1f));
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

    //public void Attack_Damage()
    //{
    //    enemy_attack.Damage();
    //}

    IEnumerator ChangeColor(Color color, float colorChangeTime)
    {
        spriteRenderer.color = color;

        yield return new WaitForSeconds(colorChangeTime);

        spriteRenderer.color = new Color(1, 1, 1);
    }

}
