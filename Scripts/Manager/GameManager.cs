using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private Transform playerTrans;
    private CanvasGroup canvasGroup;
    private Coroutine fade_rouine;

    //回到初始頁面
    public int StartMenuNum = 0;

    // Use this for initialization
    void Start ()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

        transform.Find("Canvas").gameObject.SetActive(true);
        canvasGroup = transform.Find("Canvas").GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("esc");
            GoToScene(StartMenuNum);
        }
    }

    public void LevelClear(int level)
    {
        LevelData.ClearLevel(level);
        GoToScene(StartMenuNum);
    }

    public void GoToScene(int sceneNum)
    {
        StartCoroutine(ChangeScene(sceneNum));
    }
    IEnumerator ChangeScene(int sceneNum)
    {
        PlayerStatus.isChangingScene = true;
        BlackFadeInOut(true, 0.7f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneNum);
        playerTrans.position = new Vector3(0f, 0f, 0f);
        playerTrans.GetComponent<PlayerEnergy>().ResetEnegy();
        BlackFadeInOut(false, 0.7f);
        yield return new WaitForSeconds(0.7f);
        PlayerStatus.isChangingScene = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().NewLevelInit();
    }

    public void Dead()
    {
        PlayerStatus.canControl = false;
        BlackFadeInOut(true, 0.5f);
    }

    void BlackFadeInOut(bool isFadeIn, float inTime)
    {
        if (fade_rouine != null) { StopCoroutine(fade_rouine); fade_rouine = null; }

        fade_rouine = StartCoroutine(BlackFadeIn(isFadeIn, inTime));
    }
    IEnumerator BlackFadeIn(bool isFadeIn, float inTime)
    {
        float fadeSpeed = 1f / inTime;

        if(isFadeIn) while (canvasGroup.alpha < 1f) { canvasGroup.alpha += fadeSpeed * Time.deltaTime; yield return null; }
        else while (canvasGroup.alpha > 0f) { canvasGroup.alpha -= fadeSpeed * Time.deltaTime; yield return null; }
    }

}
