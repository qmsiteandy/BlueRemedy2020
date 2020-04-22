using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ESC_Menu : MonoBehaviour
{
    public GameObject[] ESC_folder;
    private GameManager gameManager;

    public GameObject[] mian_buttons;
    public int selectedIndex = 0;
    public bool inputHold = false;

    public enum Step { mian,control,exit};
    public Step ESC_Step;



    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void OnEnable()
    {
        ResetEscMain();
    }

    void Update()
    {
        if (ESC_Step == Step.mian)
        {
            //鍵盤選擇
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("XBOX_Vertical") > 0.5f) && !inputHold)
            {
                inputHold = true;
                do
                {
                    selectedIndex -= 1;
                    if (selectedIndex < 0) selectedIndex = mian_buttons.Length - 1;
                }
                while (mian_buttons[selectedIndex].GetComponent<Button>().interactable == false);
            }
            else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("XBOX_Vertical") < -0.5f) && !inputHold)
            {
                inputHold = true;
                do
                {
                    selectedIndex += 1;
                    if (selectedIndex >= mian_buttons.Length) selectedIndex = 0;
                }
                while (mian_buttons[selectedIndex].GetComponent<Button>().interactable == false);
            }
            else if (Mathf.Abs(Input.GetAxis("XBOX_Vertical")) < 0.5f && inputHold) inputHold = false;

            //設定選擇的項目
            EventSystem.current.SetSelectedGameObject(mian_buttons[selectedIndex]);


            //進入
            if (Input.GetButtonDown("Submit"))
            {
                // EventSystem.current.currentSelectedGameObject.
                switch (selectedIndex)
                {
                    case 0:
                        gameManager.SetEscMenu(false);
                        break;
                    case 1:
                        ESC_folder[0].SetActive(false); ESC_folder[1].SetActive(true);
                        ESC_Step = Step.control;
                        break;
                    case 2:
                        ESC_folder[2].SetActive(true);
                        ESC_Step = Step.exit;
                        break;
                }
            }
        }
        else if(ESC_Step == Step.control)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                ResetEscMain();
                ESC_Step = Step.mian;
            }
        }
        else if (ESC_Step == Step.exit)
        {
            if (Input.GetButtonDown("Submit"))
            {
                PlayerStatus.canControl = true;
                Time.timeScale = 1f;

                if (SceneManager.GetActiveScene().buildIndex == SceneManager.GetSceneByName("Level_Room").buildIndex) gameManager.GoToScene(0); 
                else if (SceneManager.GetActiveScene().buildIndex >= SceneManager.GetSceneByName("Level_Room").buildIndex) gameManager.GoToScene("Level_Room");

                this.gameObject.SetActive(false);
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                ESC_folder[2].SetActive(false);
                ESC_Step = Step.mian;
            }
        }
    }

    void ResetEscMain()
    {
        for (int x = 0; x < ESC_folder.Length; x++) ESC_folder[x].SetActive(false);
        ESC_folder[0].SetActive(true);

        selectedIndex = 0;
        EventSystem.current.SetSelectedGameObject(mian_buttons[selectedIndex]);

        ESC_Step = Step.mian;
    }
}
