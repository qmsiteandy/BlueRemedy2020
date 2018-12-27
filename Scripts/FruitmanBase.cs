using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitmanBase : MonoBehaviour {

    public int ID;
    public int fressment = 0;
    public GameObject followTarget = null;
    public GameObject nextOne = null;

    public float followSpeed = 0.06f;
    public float followRange = 0.7f;
    public float jumpForce = 350.0f;
    public bool facingRight = true;
    public float dieFX_Time = 1f;

    private bool isAlive = true;
    //private bool grounded = true;
    //private Rigidbody2D rb2d;

 
    // Use this for initialization
    void Awake () {

        isAlive = true;
        fressment = FruitmanData.InfoList[ID].fressment;

        //rb2d = GetComponent<Rigidbody2D>();
    }
	

	// Update is called once per frame
	void FixedUpdate () {

        if (isAlive)
        {
            if (followTarget != null)
            {
                Follow();
                //Jump();
            }
        } 
	}

    void Update()
    {
        if (fressment <= 0 && isAlive)
        {
            StartCoroutine(Die());
        }
    }


    void Follow()
    {
        float xDiff = followTarget.transform.position.x - transform.position.x;

        if (Mathf.Abs(xDiff) > followRange)
        {
            float posNow = Mathf.Lerp(transform.position.x, followTarget.transform.position.x, followSpeed);
            transform.position = new Vector2(posNow, transform.position.y);
        }

        if (xDiff > 0 && !facingRight)
        {
            Flip();
        }
        else if (xDiff < 0 && facingRight)
        {
            Flip();
        }
    }


    /*void Jump()
    {

    }*/


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

    public void FressLoss(int loss)
    {
        if (isAlive)
        {
            fressment -= loss;
        }
    }

    public void SetTarget(GameObject target)
    {
        followTarget = target;
    }

    public void SetNextOne(GameObject next)
    {
        nextOne = next;
    }

    IEnumerator Die()
    {
        gameObject.GetComponentInParent<FruitManager>().FirstOneDie(ID);
        isAlive = false;

        float elapsed = 0;

        /*這裡要放水果死亡動畫*/

        SpriteRenderer spriderRender = this.GetComponent<SpriteRenderer>();
        spriderRender.material.color = new Color(0.4f, 0.4f, 0.4f, 1f);
        spriderRender.sortingLayerName = "back";

        while (elapsed < dieFX_Time)
        {
            elapsed += Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }
}
