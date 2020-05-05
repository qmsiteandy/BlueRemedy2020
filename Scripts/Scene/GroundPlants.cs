using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlants : MonoBehaviour {

    [Header("計算豐饒")]
    [Tooltip("是否長出並計算豐饒度")]public bool b_GrowAndCountedBlossom = true;
    private float thisGrassRange;
    private static float totalGrassRange = 0f, nowGrassRange = 0f;
    private static UI_Manager UI_manager;

    [Header("長出")]
    [HideInInspector] public bool b_isGrowFromMainFolwer = false;
    private bool b_animateGrow;
    private bool isGrowed = false;
    private Animator animator;
    private float oriYPos;
    private static float YShift = 0.5f;
    private static float appearSpeed = 0.1f;

    void Start ()
    {
        if (b_GrowAndCountedBlossom)
        {
            //---計算豐饒---
            thisGrassRange = this.GetComponent<BoxCollider2D>().size.x;
            totalGrassRange += thisGrassRange;

            if (GameObject.Find("UI_Canvas") == null) UI_manager = null;
            else UI_manager = GameObject.Find("UI_Canvas").GetComponent<UI_Manager>();

            //---草長出---
            animator = GetComponent<Animator>();
            b_animateGrow = (animator != null);

            if (!b_animateGrow)
            {
                oriYPos = transform.position.y;
                this.transform.position -= new Vector3(0f, YShift, 0f);
                this.GetComponent<BoxCollider2D>().offset = new Vector2(0f, YShift);
            } 
        }
        else
        {
            animator = GetComponent<Animator>();
            if (animator != null) animator.enabled = false;
        }

        GetComponent<Collider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (b_isGrowFromMainFolwer) return;

        if (collision.gameObject.tag == "Player" && !isGrowed && b_GrowAndCountedBlossom) Grow();
    }

    public void GrowByMainFlower()
    {
        if (b_isGrowFromMainFolwer)
            if (!isGrowed && b_GrowAndCountedBlossom)
            {
                Grow();

                GetComponent<BlossomCalculate>().AddThis();
            }
    }

    void Grow()
    {
        if (isGrowed) return;

        nowGrassRange += thisGrassRange;
        float persentage = nowGrassRange / totalGrassRange;
        if(UI_manager!=null) UI_manager.SetBlossomUI(persentage);

        if(b_animateGrow) animator.SetTrigger("grow");
        else StartCoroutine(GrassLerpGrow());

        GetComponent<Collider2D>().enabled = true;

        isGrowed = true;
    }

    IEnumerator GrassLerpGrow()
    {
        while (this.transform.position.y < oriYPos)
        {
            float newYPos = Mathf.Lerp(this.transform.position.y, oriYPos, appearSpeed);
            this.transform.position = new Vector3(this.transform.position.x, newYPos, this.transform.position.z);
            this.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0f);

            yield return null;
        }
    }
}
