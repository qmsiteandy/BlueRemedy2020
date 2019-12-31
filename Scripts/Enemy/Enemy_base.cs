using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_base : MonoBehaviour {

    GameObject target;
    Animator animator;
    SpriteRenderer spriteRenderer;  //怪物圖的render
    Vector2 centerPos;      //移動的區域中點
    public GameObject enemy;
    Enemy_Attack enemy_attack;
    public GameObject waterdrop;
    
    [Header("Move Settings")]
    public float moveRange = 4f;    //移動範圍半徑
    private float posNow = 0f;      //目前移動相對中點的位置
    private bool goRight = true;    //是否往右走
    public float trackSpeed = 6f;
    public float closeRange = 1.25f;
    private Rigidbody2D rb2d;

    [Header("Track Settings")]
    public float FollowRadius = 4.5f;
    private Collider2D[] playerCol = { null };
    private ContactFilter2D playerFilter;

    [Header("Awake Settings")]
    private float bornTime = 8f;

    [Header("Health Settings")]
    public int health;
    public int healthMax = 200;

    [Header("Action Settings")]
    public bool isTracking = false;
    public bool isAttacking;
    public bool isInjury;
    public bool isDead;
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
        health = healthMax;
        enemy = transform.GetChild(0).gameObject;
        enemy_attack = transform.GetComponentInChildren<Enemy_Attack>();
        rb2d = GetComponent<Rigidbody2D>();
        playerFilter.SetLayerMask(LayerMask.GetMask("Player"));
    }

    void Update()
    {
        if(!isDead){
            FindPlayer();

            if (isTracking)
            {
                Tracking();
            }
            else
            {
                animator.SetBool("Walk", false);
            }
        }
        //if (isBorn)
        //{
        //    bornTime -= Time.deltaTime; //重生倒數
        //    if (bornTime <= 0)
        //    {
        //        isDead = false;
        //        enemy.SetActive(true);
        //        animator.SetTrigger("Born");
        //        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        //        health = healthMax;
        //        isBorn = false;
        //        bornTime = 20f;
        //    }
        //}
    }

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

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                isDead = true;
                animator.SetTrigger("Dead");
            }
            isInjury = true;
            animator.SetTrigger("Injury");
            StartCoroutine(ChangeColor(new Color(1f, 0.3962386f, 0.3726415f), 0.1f));
        }
    }

    public void InjuryOver()
    {
        isInjury = false;
    }
    public void AttackStart()
    {
        isAttacking = true;
    }
    public void AttackOver()
    {
        isAttacking = false;
    }

    public void Dead()
    {
        
        Debug.Log(enemy.name + "die");
        GameObject water = Instantiate(waterdrop, enemy.transform.position, Quaternion.identity);
        water.GetComponent<WaterDrop>().enemy_base= this;
        water.transform.SetParent(this.transform);
        enemy.SetActive(false);
    }
  

    public void Attack()
    {
        enemy_attack.Damage();
    }

    public void NewBaby()
    {
        StartCoroutine(RebornAfterTime(bornTime));
    }

    IEnumerator RebornAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        isDead = false;
        enemy.SetActive(true);
        health = healthMax;
        animator.SetTrigger("Born");
        Debug.Log("born");
    }

    IEnumerator ChangeColor(Color color, float colorChangeTime)
    {
        spriteRenderer.color = color;

        yield return new WaitForSeconds(colorChangeTime);

        spriteRenderer.color = new Color(1, 1, 1);
    }

}


