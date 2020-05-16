using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Special_g_tornado : MonoBehaviour {

    private bool isTornadoAlive = true; //龍捲風消散時無法再捲動物品
    private float xSpeed = 0f;
    private Vector3 tornadoSpeed;
    private Vector2 groundNormal = Vector2.up;
    private Transform topPoint;
    private float attractRadius;

    public class c_ObjInTornado
    {
        public Transform transform;
        public Rigidbody2D rb2d;

        public c_ObjInTornado(Transform newTrans,Rigidbody2D new_rb2d)
        {
            transform = newTrans;
            rb2d = new_rb2d;
        }
    }
    public List<c_ObjInTornado> ObjInTornado = new List<c_ObjInTornado>();

    private Transform arrowTrans;
    private SpriteRenderer arrowRender;

    private float throwAngle = 90f;
    private float throwForce = 1000f;

    public GameObject tornaoSplash_FX;

    void Awake()
    {
        topPoint = transform.Find("topPoint").transform;
        attractRadius = GetComponent<CapsuleCollider2D>().size.x * 0.75f;

        arrowTrans = transform.Find("throw_arrow").transform;
        arrowRender = arrowTrans.GetChild(0).GetComponent<SpriteRenderer>();
        arrowRender.color = new Color(1f, 1f, 1f, 0f);
    }
    public void SetSpeed(float new_xSpeed) { xSpeed = new_xSpeed; }

    void Update()
    {
        //偵測腳下平台法向量
        groundNormal = DownHitNormal();

        //龍捲風移動
        if (groundNormal.y > 0.6f)
        {
            tornadoSpeed = new Vector3(xSpeed, xSpeed * (groundNormal.x / groundNormal.y), 0f);

            this.transform.position += tornadoSpeed * Time.deltaTime;
        }

        if (isTornadoAlive) //龍捲風還活著時才能捲動物品
        {
            //吸引範圍內物品(龍捲風外圍)
            LayerMask layerMask = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Ground & Wall") | 1 << LayerMask.NameToLayer("Platform"));
            Collider2D[] colliders = Physics2D.OverlapCapsuleAll((this.transform.position + new Vector3(0f, 2.25f, 0f)), new Vector2(11f, 7.5f), CapsuleDirection2D.Horizontal, 0f, layerMask);
            foreach (Collider2D col in colliders)
            {
                Rigidbody2D colRb2d;
                if ((colRb2d = col.GetComponent<Rigidbody2D>()) != null)
                {
                    float xDis = Mathf.Abs(col.transform.position.x - this.transform.position.x);

                    if (xDis > attractRadius)
                        colRb2d.velocity = new Vector2((this.transform.position.x - col.transform.position.x) * (1f - (xDis / 11f)) * 5f, colRb2d.velocity.y);
                }
            }

            //龍捲風捲起物品
            foreach (c_ObjInTornado obj in ObjInTornado)
            {
                if (obj.transform.position.y < topPoint.position.y)
                {
                    float xDis = Mathf.Abs(obj.transform.position.x - this.transform.position.x);

                    if (xDis > 0.5f)
                    {
                        obj.rb2d.velocity = new Vector2((this.transform.position.x - obj.transform.position.x) * 5f, obj.rb2d.velocity.y);
                    }
                    else
                    {
                        obj.rb2d.velocity = ((topPoint.position - obj.transform.position) * 5f);
                    }
                }
            }
        } 

        if (ObjInTornado.Count > 0 && arrowRender.color.a == 0f) arrowRender.DOFade(1f, 0.5f);
        else if (ObjInTornado.Count == 0 && arrowRender.color.a > 0f) arrowRender.DOFade(0f, 0f);
    }

    Vector2 DownHitNormal()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, 2f, (LayerMask.GetMask("Ground & Wall") | LayerMask.GetMask("Platform")));
        return(hit.normal);
    }

    public void Release()
    {
        GetComponent<Animator>().SetTrigger("release");

        //如果有東西在裡面就丟出
        if (ObjInTornado.Count > 0) { ThrowOut(); }
        //否則直接消散
        else { }

        isTornadoAlive = false;
        this.GetComponent<Collider2D>().enabled = false;
        ObjInTornado.Clear();
    }

    public void SetAngle(float angle)
    {
        throwAngle = angle;

        arrowTrans.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    void ThrowOut()
    {        
        foreach (c_ObjInTornado obj in ObjInTornado)
        {
            Vector2 dir = new Vector2(Mathf.Cos(throwAngle * (Mathf.PI / 180f)), Mathf.Sin(throwAngle * (Mathf.PI / 180f)));
            obj.rb2d.AddForce(dir * throwForce);

            GameObject FX = Instantiate(tornaoSplash_FX, obj.transform.position, Quaternion.Euler(0f, 0f, throwAngle));
            Destroy(FX, 1f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            ObjInTornado.Add(new c_ObjInTornado(collision.transform, collision.GetComponent<Rigidbody2D>()));

            //物品捲入後，龍捲風移動速度變慢
            xSpeed *= 0.5f;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        //如果離開的物品是在LIST中的
        foreach (c_ObjInTornado obj in ObjInTornado)
        {
            if (collision.transform == obj.transform)
            {
                Debug.Log(collision.name);
                ObjInTornado.Remove(obj);

                break;
            }
        }
    }

    //call by animation
    void Disappear()
    {
        Destroy(this.gameObject);
    }
}
