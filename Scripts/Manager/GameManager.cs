using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private Transform playerTrans;
    private CanvasGroup canvasGroup;
    private Coroutine fade_rouine;

    private bool isESC = false;

    // Use this for initialization
    void Start ()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

        transform.Find("Canvas").gameObject.SetActive(true);
        transform.Find("Vignette").gameObject.SetActive(true);

        canvasGroup = transform.Find("Canvas").GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (Input.GetButtonDown("ESC"))
        {
            if(SceneManager.GetActiveScene().buildIndex > 0)
            {
                isESC = !isESC;
                SetEscMenu(isESC);
            }
        }
    }

    public void LevelClear(int level)
    {
        LevelData.ClearLevel(level);
        GoToScene("Level_Room");
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(ChangeScene(sceneName, 9999));
    }
    public void GoToScene(int sceneNum)
    {
        StartCoroutine(ChangeScene(" ", sceneNum));
    }
    IEnumerator ChangeScene(string sceneName, int sceneNum)
    {
        PlayerStatus.canControl = false;
        BlackFadeInOut(true, 0.7f);
        yield return new WaitForSeconds(1f);

        playerTrans.GetComponent<PlayerChange>().ForceChangeForm(1);

        if (sceneNum != 9999) SceneManager.LoadScene(sceneNum);
        else SceneManager.LoadScene(sceneName);

        playerTrans.position = new Vector3(0f, 0f, 0f);
        playerTrans.GetComponent<PlayerEnergy>().ResetEnegy();
        BlackFadeInOut(false, 0.7f);
        yield return new WaitForSeconds(0.7f);
        PlayerStatus.canControl = true;
        playerTrans.GetComponent<PlayerControl>().NewLevelInit();
    }

    public void Dead()
    {
        PlayerStatus.canControl = false;
        BlackFadeInOut(true, 0.5f);
    }

    public void BlackFadeInOut(bool isFadeIn, float inTime)
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

    public void SetEscMenu(bool isEscOpen)
    {
        Time.timeScale = isEscOpen ? 0f : 1f;
        PlayerStatus.canControl = !isEscOpen;

        transform.Find("ESC_Canvas").gameObject.SetActive(isEscOpen);
    }
}
