using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassControl : MonoBehaviour {

    private static float totalGrassRange = 0f, nowGrassRange = 0f;
    private float thisGrassRange;
    private float oriYPos;
    private static float YShift = 0.5f;
    private static float appearSpeed = 0.15f;
    private bool isGrowed = false;

    private static UI_Manager UI_manager;

	void Start ()
    {
        thisGrassRange = this.GetComponent<BoxCollider2D>().size.x;
        totalGrassRange += thisGrassRange;

        oriYPos = transform.position.y;
        this.transform.position -= new Vector3(0f, YShift, 0f);
        this.GetComponent<BoxCollider2D>().offset = new Vector2(0f, YShift);

        UI_manager = GameObject.Find("UI_Canvas").GetComponent<UI_Manager>();
        //初始化UI
    }

    public void GrowGrass()
    {
        if (isGrowed) return;

        nowGrassRange += thisGrassRange;
        float persentage = nowGrassRange / totalGrassRange;
        //設定UI

        this.GetComponent<BoxCollider2D>().enabled = false;

        StartCoroutine(GrassLerp());

        isGrowed = true;
    }

    IEnumerator GrassLerp()
    {
        while (this.transform.position.y < oriYPos)
        {
            float newYPos = Mathf.Lerp(this.transform.position.y, oriYPos, appearSpeed);
            this.transform.position = new Vector3(this.transform.position.x, newYPos, this.transform.position.z);

            yield return null;
        } 
    }

}
