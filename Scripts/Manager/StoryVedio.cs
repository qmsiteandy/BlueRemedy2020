using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;

public class StoryVedio : MonoBehaviour
{
    private bool isSkiped = false;
    private bool canSkip = false;
    private GameObject skipButton;
    private Slider skipSlider;
    private float skipHoldTimeMax = 1.5f;
    private float skipHoldTime = 0f;
    private Image blackPanel;
    private GameObject BGM_Object;  //原本存在關卡中的背景音樂物件

    private enum VedioState { LevelStart, LevelEnd};
    private VedioState vedioState;

    public UnityEvent FinishEvent;

    void Awake()
    {
        skipButton = transform.Find("SkipButton").gameObject;
        skipSlider = skipButton.transform.Find("Slider").GetComponent<Slider>();
        skipSlider.value = 0f;

        transform.Find("Vedio").gameObject.SetActive(false);

        blackPanel = transform.Find("BlackPanel").GetComponent<Image>();
        blackPanel.color = new Color(0f, 0f, 0f, 0f);

        BGM_Object = GameObject.Find("BGM_Object");

        if (this.transform.IsChildOf(GameObject.Find("LevelClear_Goal").transform)) vedioState = VedioState.LevelEnd;
        else vedioState = VedioState.LevelStart;
    }

    void Start()
    {
        if(vedioState==VedioState.LevelStart) VedioStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSkiped || !canSkip) return;

        if (Input.GetButton("Submit") || Input.GetKey(KeyCode.Space))
        {
            if (skipHoldTime >= skipHoldTimeMax)
            {
                VedioEnd();
                isSkiped = true;
            }
            else
            {
                skipHoldTime += Time.deltaTime;
                if (skipHoldTime > skipHoldTimeMax) skipHoldTime = skipHoldTimeMax;
            }
        }
        else if (skipHoldTime > 0f)
        {
            skipHoldTime -= Time.deltaTime * 2f;
            if (skipHoldTime < 0f) skipHoldTime = 0f;
        }

        float skipValue = skipHoldTime / skipHoldTimeMax;
        skipSlider.value = skipValue;
    }

    public void VedioStart()
    {
        StartCoroutine(cor_VedioStart());
    }
    IEnumerator cor_VedioStart()
    {
        PlayerStatus.canControl = false;
        if (BGM_Object != null) BGM_Object.SetActive(false);

        GameObject.Find("GameManager").GetComponent<GameManager>().BlackPanelFade(1f, 0.5f);
        yield return new WaitForSeconds(1f);

        transform.Find("Vedio").gameObject.SetActive(true);
        this.GetComponent<CanvasGroup>().alpha = 0f;
        this.GetComponent<CanvasGroup>().DOFade(1f, 1f);

        skipButton.SetActive(false);
        StartCoroutine(OpenSkipBtn(3f));
    }

    public void VedioEnd()
    {
        StartCoroutine(cor_VedioEnd());

        FinishEvent.Invoke();
    }
    IEnumerator cor_VedioEnd()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().BlackPanelFade(0f, 0f);
        blackPanel.DOFade(1f, 1f);
        yield return new WaitForSeconds(1f);

        this.transform.Find("Vedio").gameObject.SetActive(false);
        skipButton.SetActive(false);

        //依據影片撥放類型，做不同的end
        if (vedioState == VedioState.LevelStart)
        {
            if (BGM_Object != null) BGM_Object.SetActive(true);

            this.GetComponent<CanvasGroup>().DOFade(0f, 1f);
            yield return new WaitForSeconds(1f);

            PlayerStatus.canControl = true;
        }
        else if(vedioState == VedioState.LevelEnd)
        {
            transform.parent.Find("LevelClear_Canvas").GetComponent<LevelClear_Menu>().StartPlay();

            this.GetComponent<Canvas>().sortingOrder = 5;
            yield return new WaitForSeconds(1f);
        }
        
        Destroy(this.gameObject);
    }

    IEnumerator OpenSkipBtn(float delay)
    {
        yield return new WaitForSeconds(delay);
        skipButton.SetActive(true);
        canSkip = true;
    }
}


