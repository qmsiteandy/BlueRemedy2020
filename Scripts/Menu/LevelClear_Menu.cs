using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelClear_Menu : MonoBehaviour {

    private float levelClearTime;
    private float blossomDegree;

    private Text levelClearTime_Text, blossomDegree_Text;

    private Animator animator;

    private bool canContinue = false;


    void Start ()
    {
        levelClearTime = GameObject.Find("Level_Timer").GetComponent<Level_Timer>().Get_LevelTime();
        blossomDegree = BlossomCalculate.GetBlossomDegree();

        levelClearTime_Text = transform.Find("Level_Time").GetComponent<Text>();
        levelClearTime_Text.text = "00:00";
        blossomDegree_Text = transform.Find("Level_Blossom").GetComponent<Text>();
        blossomDegree_Text.text = "0/100";

        animator = GetComponent<Animator>();
        animator.SetTrigger("Clear");
    }

    private void Update()
    {
        if (canContinue)
        {
            if (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Space))
            {
                if (transform.parent.Find("StoryVedio") != null)
                {
                    transform.parent.Find("StoryVedio").GetComponent<StoryVedio>().VedioStart();
                }
                else
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().GoToScene("Level_Room");
                }

                this.enabled = false;
            }
            
        }
    }

    #region TimeTextCount
    void Start_TimeTextCount()
    {
        StartCoroutine(LerpLevelClearTime());
    }
    IEnumerator LerpLevelClearTime()
    {
        animator.speed = 0f;

        int min = (int)(levelClearTime / 60f);
        int sec = (int)(levelClearTime % 60f);

        int min_text = 0;
        int sec_text = 0;

        while (min_text < min)
        {
            min_text += 1;

            SetLevelClearTime(min_text, sec_text);
            yield return new WaitForSeconds(0.02f);
        }
        while (sec_text < sec)
        {
            sec_text += 1;

            SetLevelClearTime(min_text, sec_text);
            yield return new WaitForSeconds(0.02f);
        }

        animator.speed = 1f;
    }
    void SetLevelClearTime(int min,int sec)
    {
        string x = min < 10 ? ("0" + min) : ("" + min);
        string y = sec < 10 ? ("0" + sec) : ("" + sec);
        levelClearTime_Text.text = x + ":" + y;
    }
    #endregion

    #region BlossomTextCount
    void Start_BlossomTextCount()
    {
        StartCoroutine(LerpBlossomText());
    }
    IEnumerator LerpBlossomText()
    {
        animator.speed = 0f;

        int percent = (int)(blossomDegree * 100f);

        int percent_text = 0;


        while (percent_text < percent)
        {
            percent_text += 1;
            SetBlossomText(percent_text);
            yield return new WaitForSeconds(0.02f);
        }

        animator.speed = 1f;
    }
    void SetBlossomText(int percent)
    {
        blossomDegree_Text.text = percent + "/100";
    }
    #endregion


    void CanContinue()
    {
        canContinue = true;
    }
}
