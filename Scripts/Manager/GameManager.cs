using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject blackPanel;


	// Use this for initialization
	void Start ()
    {
        GameObject.DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoToScene(int sceneNum)
    {
        StartCoroutine(ChangeScene(sceneNum));
    }
    IEnumerator ChangeScene(int sceneNum)
    {
        //Player cant move
        blackPanel.GetComponent<panel>().FadeIn();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneNum);
        blackPanel.GetComponent<panel>().FadeOut();
        yield return new WaitForSeconds(1f);
        //Player can move
    }
}
