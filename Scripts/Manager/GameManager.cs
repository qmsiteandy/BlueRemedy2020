using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private Transform playerTrans;
    private Transform blackPanel;

	// Use this for initialization
	void Start ()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        blackPanel = transform.Find("Canvas").Find("BlackPanel");
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
        blackPanel.GetComponent<panel>().FadeOut();
        yield return new WaitForSeconds(1f);
        PlayerStatus.isChangingScene = false;
    }

   
}
