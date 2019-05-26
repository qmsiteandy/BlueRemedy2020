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
    public Vector2 waterFillOriRect2D;
    private Image waterFillImg;
    public float Yshift = 550f;
    public Text waterText;

    [Header("髒污UI")]
    private int dirtyMax;
    public GameObject dirtyFill;
    private Image dirtyFillImg;



    void Start ()
    {
        waterMax = playerEnergy.waterEnergyMax;
        waterText.text = "" + waterMax;

        waterFillRect = waterFill.GetComponent<RectTransform>();
        waterFillImg = waterFill.GetComponent<Image>();

        waterFillOriRect2D = waterFillRect.anchoredPosition;

        //------

        dirtyMax = playerEnergy.dirtMax;

        dirtyFillImg = dirtyFill.GetComponent<Image>();
        dirtyFillImg.color = new Color(1f, 1f, 1f, 0f);


    }
	
	public void SetWaterUI(int waterEnergy)
    {
        waterText.text = "" + waterEnergy;
        waterFillRect.anchoredPosition = waterFillOriRect2D - new Vector2(0f, Yshift * (waterMax - waterEnergy) / waterMax);
        if (waterEnergy < waterMax * 0.2f) waterFillImg.color = new Color(1f, 0f, 0f);
        else waterFillImg.color = new Color(1f, 1f, 1f);
    }

    public void SetDirtyUI(float dirtyDegree)
    {
        //Debug.Log("dirtyDegree " + dirtyDegree);
        dirtyFillImg.color = new Color(1f, 1f, 1f, dirtyDegree);
        if(dirtyDegree>0.6f) dirtyFillImg.color = new Color(1f, 0f, 0f, dirtyDegree);
        else dirtyFillImg.color = new Color(1f, 1f, 1f, dirtyDegree);
    }
}
