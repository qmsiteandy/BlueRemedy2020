using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private Transform playerTrans;
    private Transform blackPanel;

    //回到初始頁面
    public int StartMenuNum = 0;

    // Use this for initialization
    void Start ()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        blackPanel = transform.Find("Canvas").Find("BlackPanel");
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
        blackPanel.GetComponent<panel>().FadeIn();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneNum);
        playerTrans.position = new Vector3(0f, 0f, 0f);
        playerTrans.GetComponent<PlayerEnergy>().ResetEnegy();
        blackPanel.GetComponent<panel>().FadeOut();
        yield return new WaitForSeconds(1f);
        PlayerStatus.isChangingScene = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().NewLevelInit();
    }

    public void Dead()
    {
        PlayerStatus.canControl = false;
        blackPanel.GetComponent<panel>().FadeIn();
    }
}
