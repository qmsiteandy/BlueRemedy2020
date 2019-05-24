using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {

    public PlayerEnergy playerEnergy;

    [Header("水量UI")]
    private int waterMax;
    public GameObject waterFill;
    private RectTransform waterFillRect;
    private Image waterFillImg;
    public float Yshift = 550f;
    public Text waterText;



    void Start ()
    {
        waterMax = playerEnergy.waterEnergyMax;
        waterText.text = "" + waterMax;

        waterFillRect = waterFill.GetComponent<RectTransform>();
        waterFillImg = waterFill.GetComponent<Image>();
    }
	
	public void SetWaterUI(int waterEnergy)
    {
        waterText.text = "" + waterEnergy;
        waterFillRect.anchoredPosition = new Vector2(0f, -Yshift * (waterMax - waterEnergy) / waterMax);
        if (waterEnergy < waterMax * 0.2f) waterFillImg.color = new Color(1f, 0f, 0f);
        else waterFillImg.color = new Color(1f, 1f, 1f);
    }
}
