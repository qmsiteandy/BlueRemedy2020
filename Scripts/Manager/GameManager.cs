using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour {

    [Header("DarkPanel")]
    private CanvasGroup blackPanel_canvasGroup;

    [Header("ChangeScene & Loading")]
    float BlackFadeTime = 1f;
    private CanvasGroup changeScene_canvasGroup;
    private Coroutine fade_rouine;
    private int OKAID_now = 1;

    [Header("ESC")]
    private ESC_Menu esc_menu;
    private bool isESC = false;

    // Use this for initialization
    void Awake ()
    {
        transform.Find("Vignette").gameObject.SetActive(true);

        transform.Find("Canvas").gameObject.SetActive(true);
        blackPanel_canvasGroup = transform.Find("Canvas").GetComponent<CanvasGroup>();
        blackPanel_canvasGroup.alpha = 0f;

        transform.Find("ChangeScene_Canvas").gameObject.SetActive(true);
        changeScene_canvasGroup = transform.Find("ChangeScene_Canvas").GetComponent<CanvasGroup>();
        changeScene_canvasGroup.alpha = 0f;

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


        //作弊鍵
        if (Input.GetKeyDown(KeyCode.Backspace)) LevelData.ClearLevel(100);
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
        OKAID_now = GameObject.Find("Player") ? PlayerControl.OkaID_Now : 1;

        BlossomCalculate.ResetBlossomDegree();

        changeScene_canvasGroup.DOFade(1f, BlackFadeTime);
        if(GameObject.Find("BGM_Object")) GameObject.Find("BGM_Object").GetComponent<BGM_Manager>().CloseBGMInTime(0.5f);

        yield return new WaitForSeconds(BlackFadeTime);

        StartCoroutine(Loading(sceneName, sceneNum));
    }
    IEnumerator Loading(string sceneName, int sceneNum)
    {
        AsyncOperation operation; 
        if (sceneNum != 9999) operation = SceneManager.LoadSceneAsync(sceneNum);
        else operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }

        StartCoroutine(LoadFinish());
    }
    IEnumerator LoadFinish()
    {
        if(GameObject.FindGameObjectWithTag("Player")!= null)
        {
            Transform playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
            playerTrans.GetComponent<PlayerChange>().ForceChangeForm(OKAID_now);

            if(SceneManager.GetActiveScene().buildIndex == 1) playerTrans.position = LevelDoor.get_lastEnterDoorPos();
            else playerTrans.position = Vector3.zero;

            GameObject.Find("ParallaxCamera").transform.position = Vector3.zero;
            playerTrans.GetComponent<PlayerEnergy>().ResetEnegy();
            PlayerStatus.set_inSeason(PlayerStatus.Season.none);     

            yield return new WaitForSeconds(0.5f);  //等待攝影機規定位
        }

        changeScene_canvasGroup.DOFade(0f, BlackFadeTime);

        yield return new WaitForSeconds(BlackFadeTime);

        PlayerStatus.canControl = true;
    }
    #endregion

    public void BlackPanelFade(float fadeTo, float inTime)
    {
        blackPanel_canvasGroup.DOFade(fadeTo, inTime);
    }

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
