using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitmanBase : MonoBehaviour {

    public int ID;
    public int fressment = 0;
    public GameObject followTarget = null;
    public GameObject nextOne = null;

    public float followSpeed = 0.06f;
    public float followRange = 0.8f;
    public float jumpForce = 350.0f;
    public bool facingRight = true;
    public float dieFX_Time = 1f;

    private bool isAlive = true;
    //private bool grounded = true;
    //private Rigidbody2D rb2d;
    private SpriteRenderer spriderRender;
    private RectTransform fressCanvasRect;
    private Slider fressSlider;
    public int midFress, lowFress;

 
    // Use this for initialization
    void Awake () {

        isAlive = true;
        fressment = FruitmanData.InfoList[ID].fressment;

        //rb2d = GetComponent<Rigidbody2D>();
        spriderRender = this.GetComponent<SpriteRenderer>();

        Transform fressCanvas = gameObject.transform.GetChild(0);
        fressCanvas.GetComponent<Canvas>().sortingOrder = spriderRender.sortingOrder;
        fressCanvasRect = fressCanvas.GetComponent<RectTransform>();

        fressSlider = fressCanvas.GetChild(0).GetComponent<Slider>();
        fressSlider.value = fressSlider.maxValue = fressment;

        midFress = (int)(0.5 * fressment);
        lowFress = (int)(0.15 * fressment);

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

            gameObject.transform.GetChild(0).gameObject.SetActive(false);
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

        Vector3 rectScale = fressCanvasRect.localScale;
        rectScale.x *= -1;
        fressCanvasRect.localScale = rectScale;
    }

    public void FressLoss(int loss)
    {
        if (isAlive)
        {
            fressment -= loss;

            fressSlider.value = fressment;
            //if(fressment<midFress)
            //else if(fressment<lowFress)
        }
    }

    public void SetTarget(GameObject target)
    {
        followTarget = target;

        if (target.tag == "Player")
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
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
