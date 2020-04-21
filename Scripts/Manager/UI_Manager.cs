using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {

    [Header("能量設定")]
    private PlayerEnergy playerEnergy;

    [Header("水量UI")]
    public float Yshift = 550f;
    private Transform waterUI;
    private int waterMax;
    private RectTransform waterFillRect;
    private Vector2 waterFillOriRect2D;
    private Image waterFillImg;
    
    private Text waterText;

    [Header("髒污UI")]
    private Transform dirtyUI;
    private int dirtyMax;
    private Image dirtyFillImg;
    private Text dirtyText;

    [Header("繁盛UI")]
    public float showoffTime = 6f;
    public float switchSpeed = 0.1f;
    private Transform blossomUI;
    private CanvasGroup blossomUI_canvasGroup;
    private Slider blossomSlider;
    private float elapsed = 0f;
    private bool showingOn = true, isTurnedOn = true, isSwitching = false;
    private float blossomUI_Alpha = 1f;    

    void Awake ()
    {
        //---能量設定---
        playerEnergy = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEnergy>();

        //---水量---
        waterUI = transform.Find("WaterUI"); 
        if (waterUI != null)
        {
            waterMax = playerEnergy.waterEnergyMax;

            waterText = waterUI.Find("text").GetComponent<Text>();
            waterText.text = "" + waterMax;

            Transform waterFill = waterUI.Find("mask").GetChild(0);
            waterFillRect = waterFill.GetComponent<RectTransform>();
            waterFillImg = waterFill.GetComponent<Image>();
            waterFillImg.color = new Color(1f, 1f, 1f, 1f);

            waterFillOriRect2D = waterFillRect.anchoredPosition;
        }

        //---髒污---
        dirtyUI = transform.Find("DirtyUI"); 
        if (dirtyUI != null)
        {
            dirtyMax = playerEnergy.dirtMax;

            Transform dirtyFill = dirtyUI.Find("mask").GetChild(0);
            
            dirtyFillImg = dirtyFill.GetComponent<Image>();
            dirtyFillImg.color = new Color(1f, 1f, 1f, 0f);

            dirtyText = dirtyUI.Find("text").GetComponent<Text>();
            dirtyText.text = 0 + "%";
        }

        //---繁盛---
        blossomUI = transform.Find("BlossomUI");
        if (blossomUI != null)
        {
            blossomSlider = blossomUI.Find("slider").GetComponent<Slider>();
            blossomSlider.value = 0f;
            blossomUI_canvasGroup = blossomUI.GetComponent<CanvasGroup>();
            blossomUI_canvasGroup.alpha = 1f;
        } 
    }

    void Update()
    {
        //---繁盛UI開啟&隱藏過程---
        if (blossomUI != null)
        {
            if (isTurnedOn) elapsed += Time.deltaTime;
            if (elapsed > showoffTime) { showingOn = false; isSwitching = true; elapsed = 0f; }

            if (!isSwitching) return;
            if (showingOn)
            {
                if (blossomUI_Alpha < 1f)
                {
                    blossomUI_Alpha = Mathf.Lerp(blossomUI_Alpha, 1f, switchSpeed * 3f);
                    if (blossomUI_Alpha > 0.95f) blossomUI_Alpha = 1f;
                    blossomUI_canvasGroup.alpha = blossomUI_Alpha;
                }
                else if (blossomUI_Alpha == 1f) isTurnedOn = true;
            }
            else
            {
                if (blossomUI_Alpha > 0f)
                {
                    blossomUI_Alpha = Mathf.Lerp(blossomUI_Alpha, 0, switchSpeed);
                    if (blossomUI_Alpha < 0.05f) blossomUI_Alpha = 0f;
                    blossomUI_canvasGroup.alpha = blossomUI_Alpha;
                }
                else if (blossomUI_Alpha == 0f) isTurnedOn = false;
            }
        } 
    }

    //---水量---
    public void SetWaterUI(int waterEnergy)
    {
        if (waterUI == null) return;

        //Debug.Log("waterEnergy " + waterEnergy);

        waterText.text = "" + waterEnergy;
        waterFillRect.anchoredPosition = waterFillOriRect2D - new Vector2(0f, Yshift * (waterMax - waterEnergy) / waterMax);
        if (waterEnergy < waterMax * 0.2f) waterFillImg.color = new Color(1f, 0f, 0f);
        else waterFillImg.color = new Color(1f, 1f, 1f);
    }

    //---髒污---
    public void SetDirtyUI(float dirtyDegree)
    {
        
        if (dirtyUI == null) return;

        //Debug.Log("dirtyDegree " + dirtyDegree);

        dirtyFillImg.color = new Color(1f, 1f, 1f, dirtyDegree);
        if(dirtyDegree>0.6f) dirtyFillImg.color = new Color(1f, 0f, 0f, dirtyDegree);
        else dirtyFillImg.color = new Color(1f, 1f, 1f, dirtyDegree);

        dirtyText.text = (int)(dirtyDegree * 100f) + "%";
    }

    //---繁盛---
    public void SetBlossomUI(float blossomPersentage)
    {
        if (blossomUI == null) return;

        blossomSlider.value = blossomPersentage;
        showingOn = true; isSwitching = true;
    }
}
