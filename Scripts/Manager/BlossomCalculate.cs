using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlossomCalculate : MonoBehaviour {

    [Header("計算豐饒")]
    [Tooltip("是否長出並計算豐饒度")]
    private bool isAdded = false;
    private float thisGrassRange;
    private static float totalGrassRange = 0f, nowGrassRange = 0f;
    private static UI_Manager UI_manager;
    private static float persentage = 0f;

    // Use this for initialization
    void Start ()
    {
        //---計算豐饒---
        thisGrassRange = this.GetComponent<BoxCollider2D>().size.x;
        totalGrassRange += thisGrassRange;

        if (GameObject.Find("UI_Canvas") == null) UI_manager = null;
        else UI_manager = GameObject.Find("UI_Canvas").GetComponent<UI_Manager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isAdded) AddThis();
    }

    public void AddThis()
    {
        if (isAdded) return;
        
        nowGrassRange += thisGrassRange;
        persentage = nowGrassRange / totalGrassRange;
        if (persentage > 0.99f) persentage = 1f;

        if (UI_manager != null) UI_manager.SetBlossomUI(persentage);

        isAdded = true;
    }

    static public void ResetBlossomDegree()
    {
        totalGrassRange = 0f; nowGrassRange = 0f;
    }
    static public float GetBlossomDegree()
    {
        return (persentage);
    }
}
