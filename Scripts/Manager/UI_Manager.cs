using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Manager : MonoBehaviour {

    [Header("能量設定")]
    private PlayerEnergy playerEnergy;

    [Header("水量UI")]
    private Transform water_UI;
    private int waterMax;
    private Slider waterSlider;

    [Header("乾淨UI")]
    private Transform purity_UI;
    private Slider puritySlider;

    [Header("繁盛UI")]
    public float showoffTime = 6f;
    public float switchSpeed = 0.1f;
    private Transform blossom_UI;
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
        water_UI = transform.Find("Water_UI"); 
        if (water_UI != null)
        {
            waterMax = playerEnergy.waterEnergyMax;

            waterSlider = water_UI.Find("Slider").GetComponent<Slider>();
        }

        //---髒污---
        purity_UI = transform.Find("Purity_UI"); 
        if (purity_UI != null)
        {
            puritySlider = purity_UI.Find("Slider").GetComponent<Slider>();
        }

        //---繁盛---
        blossom_UI = transform.Find("BlossomUI");
        if (blossom_UI != null)
        {
            blossomSlider = blossom_UI.Find("slider").GetComponent<Slider>();
            blossomSlider.value = 0f;
            blossomUI_canvasGroup = blossom_UI.GetComponent<CanvasGroup>();
            blossomUI_canvasGroup.alpha = 1f;
        } 
    }

    void Update()
    {
        //---繁盛UI開啟&隱藏過程---
        if (blossom_UI != null)
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
        if (water_UI == null) return;

        waterSlider.DOValue((float)waterEnergy / waterMax, 0.3f);
    }

    //---髒污---
    public void SetDirtyUI(float dirtyDegree)
    {
        if (purity_UI == null) return;

        puritySlider.DOValue(dirtyDegree, 0.3f);
    }

    //---繁盛---
    public void SetBlossomUI(float blossomPersentage)
    {
        if (blossom_UI == null) return;

        blossomSlider.value = blossomPersentage;
        showingOn = true; isSwitching = true;
    }
}
