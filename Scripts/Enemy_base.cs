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
    public float moveRange = 3f;    //移動範圍半徑
    private float posNow = 0f;      //目前移動相對中點的位置
    private bool goRight = true;    //是否往右走
    public float trackSpeed = 1.5f;
    public float closeRange = 1.25f;

    [Header("Awake Settings")]
    public float bornTime = 8f;

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
    }

    void Update()
    {
        if (isTracking)
        {
            Tracking();
        }

        if (isBorn)
        {
            bornTime -= Time.deltaTime; //重生倒數
            if (bornTime <= 0)
            {
                isDead = false;
                enemy.SetActive(true);
                animator.SetTrigger("Born");
                gameObject.GetComponent<CircleCollider2D>().enabled = true;
                health = healthMax;
                isBorn = false;
                bornTime = 8f;
            }
        }
    }


    IEnumerator ChangeColor(Color color, float colorChangeTime)
    {
        spriteRenderer.color = color;

        yield return new WaitForSeconds(colorChangeTime);

        spriteRenderer.color = new Color(1, 1, 1);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            target = collision.gameObject;

            isTracking = true;
        }
    }

    void Tracking()
    {
        if (target != null && !isAttacking)
        {
            animator.SetBool("Walk", true);
            Vector3 diff = new Vector3(target.transform.position.x - transform.position.x, 0, 0);
            if (Mathf.Abs(diff.x) <= closeRange) return;
            posNow = Mathf.Lerp(posNow, target.transform.position.x - centerPos.x, trackSpeed * Time.deltaTime);
            posNow = Mathf.Clamp(posNow, -moveRange, moveRange);
            transform.position = new Vector3(centerPos.x + posNow, centerPos.y, 0f);

            float face = Mathf.Sign(diff.x);
            goRight = (face >= 0) ? true : false;
            Vector3 faceVec = new Vector3(face, 1, 1);
            transform.localScale = faceVec;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            target = null;

            isTracking = false;

            animator.SetBool("Walk", false);
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
                animator.SetTrigger("Dead");
            }
            isInjury = true;
            animator.SetTrigger("Injury");
            StartCoroutine(ChangeColor(new Color(1, 0, 0), 0.1f));
            Debug.Log("Takedamage:" + health);
        }
    }

    public void InjuryOver()
    {
        isInjury = false;
    }
    public void AttackOver()
    {
        isAttacking = false;
    }

    public void Dead()
    {
        isDead = true;
        Debug.Log(enemy.name + "die");
        GameObject water = Instantiate(waterdrop, enemy.transform.position, Quaternion.identity);
        water.GetComponent<WaterDrop>().enemy_base= this;
        enemy.SetActive(false);
    }
    public void FollowClose()
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }


    public void Attack()
    {
        enemy_attack.Damage();
    }

}


