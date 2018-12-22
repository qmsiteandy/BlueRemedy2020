using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CmdCanvasControl : MonoBehaviour {

    private Image CmdBack;
    private Color oriBack;
    private Image[] CmdImg= { null, null, null, null };
    private RectTransform[] CmdRect = { null, null, null, null };

    private Transform player;
    private Vector3 offset;
    private RectTransform rectTransform;

    private GameObject timeSliderOB;
    private Slider timeSlider;
    private bool isCmding = false;

    // Use this for initialization
    void Start () {

        //-------------UI follow主角--------------------

        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        rectTransform = GetComponent<RectTransform>();

        offset = rectTransform.anchoredPosition3D - player.position;

        //-------------UI 變化設定--------------------
        
        CmdBack = gameObject.transform.GetChild(0).GetComponent<Image>();
        oriBack = CmdBack.color;

        CmdImg[0] = gameObject.transform.GetChild(1).GetComponent<Image>();
        CmdImg[1] = gameObject.transform.GetChild(2).GetComponent<Image>();
        CmdImg[2] = gameObject.transform.GetChild(3).GetComponent<Image>();
        CmdImg[3] = gameObject.transform.GetChild(4).GetComponent<Image>();
        CmdRect[0] = gameObject.transform.GetChild(1).GetComponent<RectTransform>();
        CmdRect[1] = gameObject.transform.GetChild(2).GetComponent<RectTransform>();
        CmdRect[2] = gameObject.transform.GetChild(3).GetComponent<RectTransform>();
        CmdRect[3] = gameObject.transform.GetChild(4).GetComponent<RectTransform>();

        //-------------UI 變化設定--------------------
        timeSliderOB = gameObject.transform.GetChild(5).gameObject;
        timeSlider = timeSliderOB.GetComponent<Slider>();
        timeSlider.maxValue = FruitmanCall.cmdTimeLimit;
    }

    // Update is called once per frame
    void Update ()
    {
        rectTransform.anchoredPosition3D = player.position + offset;

        if(isCmding) timeSlider.value += Time.deltaTime;
    }

    public void CmdBackOpen()
    {
        CmdBack.enabled = true;
        timeSliderOB.SetActive(true);

        isCmding = true;

    }

    public void CmdShow(int index, int angle)
    {
        CmdRect[index].localRotation = (Quaternion.Euler(0f, 0f, (float)angle));
        CmdImg[index].enabled = true;
    }

    public void CmdClose()
    {
        CmdBack.enabled = false;

        CmdBack.color = oriBack;

        for (int x=0; x < CmdImg.Length; x++)
        {
            CmdImg[x].enabled = false;
            CmdRect[x].localRotation = (Quaternion.Euler(0f, 0f, 0f));
        }

        timeSliderOB.SetActive(false);
        timeSlider.value = 0f;

        isCmding = false;
    }

    public void CmdSuccess()
    {
        CmdBack.color = new Color(0f, 1f, 0f, 0.5f);

        timeSlider.value = timeSlider.maxValue;
    }

    public void CmdFail()
    {
        CmdBack.color = new Color(1f, 0f, 0f, 0.5f);

        timeSlider.value = timeSlider.maxValue;
    }

}
