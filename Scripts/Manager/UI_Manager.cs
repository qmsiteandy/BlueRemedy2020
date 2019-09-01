using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {

    [Header("黑框邊界")]
    public GameObject darkPanel;

    [Header("水量UI")]
    public GameObject waterFill;
    private int waterMax;
    private RectTransform waterFillRect;
    private Vector2 waterFillOriRect2D;
    private Image waterFillImg;
    public float Yshift = 550f;
    public Text waterText;

    [Header("髒污UI")]
    public GameObject dirtyFill;
    private int dirtyMax;
    private Image dirtyFillImg;

    [Header("繁盛UI")]
    public Slider blossomSlider;
    private float elapsed = 0f;
    private bool showingOn = true, isTurnedOn = true, isSwitching=false;
    private float blossomUI_Alpha = 1f;
    public float showoffTime = 6f;
    public float switchSpeed = 0.1f;
    public Image[] blossomUI_Img = { null, null, null };


    [Header("能量設定")]
    public PlayerEnergy playerEnergy;

    void Start ()
    {
        darkPanel.SetActive(true);

        //---水量---
        waterMax = playerEnergy.waterEnergyMax;
        waterText.text = "" + waterMax;

        waterFillRect = waterFill.GetComponent<RectTransform>();
        waterFillImg = waterFill.GetComponent<Image>();

        waterFillOriRect2D = waterFillRect.anchoredPosition;

        //---髒污---

        dirtyMax = playerEnergy.dirtMax;

        dirtyFillImg = dirtyFill.GetComponent<Image>();
        dirtyFillImg.color = new Color(1f, 1f, 1f, 0f);

        //---繁盛---

        blossomSlider.value = 0f;
    }

    void Update()
    {   
        //---繁盛---
        if (isTurnedOn) elapsed += Time.deltaTime;
        if (elapsed > showoffTime) { showingOn = false; isSwitching = true; elapsed = 0f; }

        if (!isSwitching) return;
        if (showingOn)
        {
            if (blossomUI_Alpha < 1f)
            {
                blossomUI_Alpha = Mathf.Lerp(blossomUI_Alpha, 1f, switchSpeed * 3f); 
                if (blossomUI_Alpha > 0.95f) blossomUI_Alpha = 1f;
                SetBlossomUI_Alpfa(blossomUI_Alpha);
            }
            else if (blossomUI_Alpha == 1f) isTurnedOn = true;
        }
        else
        {
            if (blossomUI_Alpha > 0f)
            {
                blossomUI_Alpha = Mathf.Lerp(blossomUI_Alpha, 0, switchSpeed); 
                if (blossomUI_Alpha < 0.05f) blossomUI_Alpha = 0f;
                SetBlossomUI_Alpfa(blossomUI_Alpha);
            }
            else if (blossomUI_Alpha == 0f) isTurnedOn = false;
        }

        //------
    }

    //---水量---
    public void SetWaterUI(int waterEnergy)
    {
        waterText.text = "" + waterEnergy;
        waterFillRect.anchoredPosition = waterFillOriRect2D - new Vector2(0f, Yshift * (waterMax - waterEnergy) / waterMax);
        if (waterEnergy < waterMax * 0.2f) waterFillImg.color = new Color(1f, 0f, 0f);
        else waterFillImg.color = new Color(1f, 1f, 1f);
    }

    //---髒污---
    public void SetDirtyUI(float dirtyDegree)
    {
        //Debug.Log("dirtyDegree " + dirtyDegree);
        dirtyFillImg.color = new Color(1f, 1f, 1f, dirtyDegree);
        if(dirtyDegree>0.6f) dirtyFillImg.color = new Color(1f, 0f, 0f, dirtyDegree);
        else dirtyFillImg.color = new Color(1f, 1f, 1f, dirtyDegree);
    }

    //---繁盛---
    public void SetBlossomUI(float blossomPersentage)
    {
        blossomSlider.value = blossomPersentage;
        showingOn = true; isSwitching = true;
    }

    void SetBlossomUI_Alpfa(float new_alpha)
    {
        for (int i = 0; i < blossomUI_Img.Length; i++) blossomUI_Img[i].color = new Color(1f, 1f, 1f, new_alpha);
    }
}
