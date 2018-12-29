using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitmanCall : MonoBehaviour {

    //----------設定內容--------------------
    public static float cmdTimeLimit = 2f;
    public UIRemind UI_Remind;

    private PlayerControl playerControl;
    private FruitManager fruitManager;
    private CmdCanvasControl cmdCanvasControl;
    private int[] cmdIn = { 0, 0, 0, 0 };
    private int index = 0;
    private float elapsed = 0f;
    private bool isInputting = false;
    private int fruitPrefSum;


    void Start () {
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        fruitManager = GetComponent<FruitManager>();
        cmdCanvasControl=gameObject.transform.GetChild(0).GetComponent<CmdCanvasControl>();

        fruitPrefSum = GetComponent<FruitManager>().fruitmanPrefab.Length;
    }
	
	void Update () {
        if (Input.GetButtonDown("Call"))
        {
            isInputting = true;
            playerControl.canMove = false;

            cmdCanvasControl.CmdBackOpen();
        }

        if (isInputting)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                cmdIn[index] = 1;
                cmdCanvasControl.CmdShow(index, 0);

                index++;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                cmdIn[index] = 2;
                cmdCanvasControl.CmdShow(index, 180);

                index++;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                cmdIn[index] = 3;
                cmdCanvasControl.CmdShow(index, 90);

                index++;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                cmdIn[index] = 4;
                cmdCanvasControl.CmdShow(index, -90);

                index++;
            }

            elapsed += Time.deltaTime;

            if (Input.GetKeyUp(KeyCode.LeftShift) || index >= 4 || elapsed > cmdTimeLimit)
            {
                isInputting = false;
                index = 0;
                elapsed = 0f;

                CompareCmd();
            }
        }

        if (Input.GetButtonUp("Call"))
        {
            playerControl.canMove = true;

            cmdCanvasControl.CmdClose();
        } 
    }

    void CompareCmd()
    {
        int matchId = 9999;

        for (int id = 1; id < fruitPrefSum; id++)
        {
            int matchCount = 0;

            for (int x = 0; x < 4; x++)
            {
                if (cmdIn[x] == FruitmanData.InfoList[id].cmd[x]) { matchCount++; }
                //Debug.Log("fruitCmd[" + id + "][" + x + "]=" + fruitCmd[id][x] + " cmdIn[" + x + "]=" + cmdIn[x] + "matchCount=" + matchCount);
            }

            if (matchCount >= 4) { matchId = id; break; }
        }

        cmdIn[0] = cmdIn[1] = cmdIn[2] = cmdIn[3] = 0;
        
        if (matchId != 9999)
        {
            if (FruitmanData.InfoList[matchId].can_call)
            {
                bool isSuccess = fruitManager.Grow(matchId);

                if (isSuccess)
                {
                    cmdCanvasControl.CmdColor(new Color(0f, 1f, 0f, 0.5f));
                }
                else
                {
                    UI_Remind.ShortRemind("水果籃空間不足");
                    cmdCanvasControl.CmdColor(new Color(1f, 0f, 0f, 0.5f));
                }

            }
            else
            {
                UI_Remind.ShortRemind("此水果不可召喚");
                cmdCanvasControl.CmdColor(new Color(0.5f, 0.7f, 0f, 0.5f));
            }
            
        }
        else
        {
            UI_Remind.ShortRemind("指令錯誤");
            cmdCanvasControl.CmdColor(new Color(1f, 0f, 0f, 0.5f));
        }
    }

}
