using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelClearTrigger : MonoBehaviour {

    private bool isClear = false;

    private void Awake()
    {
        if (transform.Find("StoryVedio") != null)
        {
            transform.Find("StoryVedio").gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(isClear == false)
            {
                Clear();
            }
        }
    }

    void Clear()
    {
        PlayerStatus.canControl = false;
        isClear = true;

        GameObject.Find("GameManager").GetComponent<GameManager>().LevelClear(SceneManager.GetActiveScene().buildIndex);

        if (transform.Find("StoryVedio") != null)
        {
            transform.Find("StoryVedio").gameObject.SetActive(true);
            transform.Find("StoryVedio").GetComponent<StoryVedio>().VedioStart();
        }
        else transform.Find("LevelClear_Canvas").GetComponent<LevelClear_Menu>().StartPlay();
    }
}
