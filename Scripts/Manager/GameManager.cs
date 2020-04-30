using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [Header("ChangeScene & Loading")]
    float BlackFadeTime = 0.7f;
    private CanvasGroup canvasGroup;
    private Coroutine fade_rouine;
    private Text loadingText;

    [Header("ESC")]
    private ESC_Menu esc_menu;
    private bool isESC = false;

    // Use this for initialization
    void Awake ()
    {
        transform.Find("Canvas").gameObject.SetActive(true);
        transform.Find("Vignette").gameObject.SetActive(true);

        loadingText = this.transform.Find("Canvas").Find("Text").GetComponent<Text>();

        canvasGroup = transform.Find("Canvas").GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        esc_menu = transform.Find("ESC_Canvas").GetComponent<ESC_Menu>();
    }

    void Update()
    {
        if (Input.GetButtonDown("ESC"))
        {
            if (SceneManager.GetActiveScene().buildIndex > 0)
            {
                isESC = !isESC;
                SetEscMenu(isESC);
            }
        }
    }

    #region ChangeScene & Loading
    public void GoToScene(string sceneName)
    {
        StartCoroutine(LoadStart(sceneName, 9999));
    }
    public void GoToScene(int sceneNum)
    {
        StartCoroutine(LoadStart(" ", sceneNum));
    }
    IEnumerator LoadStart(string sceneName, int sceneNum)
    {
        PlayerStatus.canControl = false;
        BlackFadeInOut(true, BlackFadeTime);
        if(GameObject.Find("BGM_Object")) GameObject.Find("BGM_Object").GetComponent<BGM_Manager>().CloseBGMInTime(0.5f);

        loadingText.text = 0 + "%";

        yield return new WaitForSeconds(BlackFadeTime);

        StartCoroutine(Loading(sceneName, sceneNum));
    }
    public void BlackFadeInOut(bool isFadeIn, float inTime)
    {
        if (fade_rouine != null) { StopCoroutine(fade_rouine); fade_rouine = null; }

        fade_rouine = StartCoroutine(BlackFadeIn(isFadeIn, inTime));
    }
    IEnumerator BlackFadeIn(bool isFadeIn, float inTime)
    {
        float fadeSpeed = 1f / inTime;

        if (isFadeIn) while (canvasGroup.alpha < 1f) { canvasGroup.alpha += fadeSpeed * Time.deltaTime; yield return null; }
        else while (canvasGroup.alpha > 0f) { canvasGroup.alpha -= fadeSpeed * Time.deltaTime; yield return null; }
    }
    IEnumerator Loading(string sceneName, int sceneNum)
    {
        AsyncOperation operation; 
        if (sceneNum != 9999) operation = SceneManager.LoadSceneAsync(sceneNum);
        else operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            loadingText.text = (int)(Mathf.Clamp01(operation.progress / 0.9f) * 100f) + "%";
            yield return null;
        }

        StartCoroutine(LoadFinish());
    }
    IEnumerator LoadFinish()
    {
        if(GameObject.FindGameObjectWithTag("Player")!= null)
        {
            Transform playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
            playerTrans.GetComponent<PlayerChange>().ForceChangeForm(1);

            if(SceneManager.GetActiveScene().buildIndex == 1) playerTrans.position = LevelDoor.lastEnterPos;
            else playerTrans.position = Vector3.zero;

            GameObject.Find("ParallaxCamera").transform.position = Vector3.zero;
            playerTrans.GetComponent<PlayerEnergy>().ResetEnegy();
            PlayerStatus.set_inSeason(PlayerStatus.Season.none);
            BlossomCalculate.ResetBlossomDegree();

            yield return new WaitForSeconds(0.5f);  //等待攝影機規定位
        }

        BlackFadeInOut(false, BlackFadeTime);

        yield return new WaitForSeconds(BlackFadeTime);

        PlayerStatus.canControl = true;
    }

    #endregion

    public void LevelClear(int level)
    {
        LevelData.ClearLevel(level);
    }
    public void Dead()
    {
        PlayerStatus.canControl = false;
        transform.Find("Dead_Canvas").gameObject.SetActive(true);
    }

    public void SetEscMenu(bool isEscOpen)
    {
        if (isEscOpen)
        {
            esc_menu.gameObject.SetActive(true);

            Time.timeScale = 0f;
            PlayerStatus.canControl = false;
        }
        else
        {
            if (esc_menu.ESC_Step == ESC_Menu.Step.mian)
            {
                esc_menu.gameObject.SetActive(false);

                Time.timeScale = 1f;
                PlayerStatus.canControl = true;
            } 
        }

        isESC = isEscOpen;
    }
}
