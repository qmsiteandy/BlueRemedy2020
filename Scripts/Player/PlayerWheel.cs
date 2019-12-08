using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWheel : MonoBehaviour {

    [Header("轉輪物件")]
    private SpriteRenderer wheelLight;
    private Transform wheelBack;
    private bool wheelShow = false;
    private SpriteRenderer[] selected_light = { null, null, null };
    private SpriteRenderer[] arrows = { null, null, null };
    private int lightIndex = 1;
    private PlayerChange playerChange;


    void Start()
    {
        playerChange = transform.parent.GetComponent<PlayerChange>();

        for (int i = 0; i < 3; i++) selected_light[i] = transform.GetChild(0).transform.GetChild(i).GetComponent<SpriteRenderer>();
        for (int i = 0; i < 3; i++) arrows[i] = transform.GetChild(1).transform.GetChild(i).GetComponent<SpriteRenderer>();
        wheelBack = this.transform.GetChild(2).transform;

        for (int i = 0; i < 3; i++)
        {
            selected_light[i].color = new Color(1f, 1f, 1f, 0f);
            arrows[i].color = new Color(1f, 1f, 1f, 0f);
        }
        wheelBack.gameObject.SetActive(false);
    }

    public void WheelShow()
    {
        for (int i = 0; i < 3; i++) selected_light[i].color = new Color(1f, 1f, 1f, 0f);
        selected_light[lightIndex].color = new Color(1f, 1f, 1f, 1f);

        for (int i = 0; i < 3; i++) arrows[i].color = new Color(1f, 1f, 1f, 0.9f);
        arrows[lightIndex].color = new Color(1f, 1f, 1f, 0.1f);

        wheelBack.gameObject.SetActive(true);

        wheelShow = true;
    }

    public void WheelDisappear()
    {
        for (int i = 0; i < 3; i++)
        {
            selected_light[i].color = new Color(1f, 1f, 1f, 0f);
            arrows[i].color = new Color(1f, 1f, 1f, 0f);
        }
        wheelBack.gameObject.SetActive(false);

        wheelShow = false;
    }

    public void WheelIndexSelect(int index)
    {
        lightIndex = index;
    }


    public void LightFlash(int index)
    {
        StartCoroutine(Flash(index));
    }
    IEnumerator Flash(int index)
    {
        float flashDelay = 0.1f;
        Color oriColor = selected_light[index].color;

        if (wheelShow) selected_light[index].color = new Color(oriColor.r, oriColor.g, oriColor.b, 0.2f);
        yield return new WaitForSeconds(flashDelay);
        if (wheelShow) selected_light[index].color = new Color(oriColor.r, oriColor.g, oriColor.b, oriColor.a);
        yield return new WaitForSeconds(flashDelay);
        if (wheelShow) selected_light[index].color = new Color(oriColor.r, oriColor.g, oriColor.b, 0.2f);
        yield return new WaitForSeconds(flashDelay);
        if (wheelShow) selected_light[index].color = new Color(oriColor.r, oriColor.g, oriColor.b, oriColor.a);

    }
}
