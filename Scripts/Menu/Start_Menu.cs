using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Start_Menu : MonoBehaviour {

    public GameObject[] buttons;

    public int selectedIndex = 0;
    private bool inputHold = false;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(buttons[selectedIndex]);
    }

    void Update()
    {
        //鍵盤選擇
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("XBOX_Vertical") > 0.5f) && !inputHold)
        {
            inputHold = true;
            do
            {
                selectedIndex -= 1;
                if (selectedIndex < 0) selectedIndex = buttons.Length - 1; 

            }
            while (buttons[selectedIndex].GetComponent<Button>().interactable == false);
        }
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("XBOX_Vertical") < -0.5f) && !inputHold)
        {
            inputHold = true;
            do
            {
                selectedIndex += 1;
                if (selectedIndex >= buttons.Length) selectedIndex = 0;
            }
            while (buttons[selectedIndex].GetComponent<Button>().interactable == false);
        }
        else if (Mathf.Abs(Input.GetAxis("XBOX_Vertical")) < 0.5f && inputHold) inputHold = false;

        //設定選擇的項目
        EventSystem.current.SetSelectedGameObject(buttons[selectedIndex]);


        //進入
        if (Input.GetButtonDown("Submit"))
        {
            switch (selectedIndex)
            {
                case 0:
                    ChangeScene();
                    break;
                case 1:
                    break;
                case 2:
                    ExitScene();
                    break;
            }
        }
    }

    void ChangeScene()
    {
        //SceneManager.LoadScene(0);
        GameObject.Find("GameManager").GetComponent<GameManager>().GoToScene("Level_Room");
    }

    void ExitScene()
    {
        Application.Quit();
    }
}
