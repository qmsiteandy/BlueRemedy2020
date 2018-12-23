using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitmanCall : MonoBehaviour {

    //----------召喚相關資訊--------------------

    //水果指令
    private int[][] fruitCmd = {
        new int[] {0,0,0,0},
        new int[] {1,1,1,1},
        new int[] {1,2,1,2},
        new int[] {1,2,3,4}
    };
    //可否召喚該水果
    private bool[] canCall = {
        false,
        true,
        true,
        false
    };

    //----------設定內容--------------------
    public static float cmdTimeLimit = 2f;

    private PlayerControl playerControl;
    private FruitManager fruitManager;
    private CmdCanvasControl cmdCanvasControl;
    private int[] cmdIn = { 0, 0, 0, 0 };
    private int index = 0;
    private float elapsed = 0f;
    private bool isInputting = false;


    void Start () {
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        fruitManager = GetComponent<FruitManager>();
        cmdCanvasControl=gameObject.transform.GetChild(0).GetComponent<CmdCanvasControl>();
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isInputting = true;
            playerControl.allCanDo = false;

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

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerControl.allCanDo = true;

            cmdCanvasControl.CmdClose();
        } 
    }

    void CompareCmd()
    {
        int matchId = 9999;

        for (int id = 1; id < fruitCmd.Length; id++)
        {
            int matchCount = 0;

            for (int x = 0; x < 4; x++)
            {
                if (cmdIn[x] == fruitCmd[id][x]) { matchCount++; }
                //Debug.Log("fruitCmd[" + id + "][" + x + "]=" + fruitCmd[id][x] + " cmdIn[" + x + "]=" + cmdIn[x] + "matchCount=" + matchCount);
            }

            if (matchCount >= 4) { matchId = id; break; }
        }

        cmdIn[0] = cmdIn[1] = cmdIn[2] = cmdIn[3] = 0;
        
        if (matchId != 9999)
        {
            if (canCall[matchId])
            {
                if (fruitManager.count >= fruitManager.countLimit)
                {
                    Debug.Log("已達召喚數量上限");
                    cmdCanvasControl.CmdColor(new Color(1f, 0f, 0f, 0.5f));
                }
                else
                {
                    fruitManager.Grow(matchId);

                    cmdCanvasControl.CmdColor(new Color(0f, 1f, 0f, 0.5f));
                }
            }
            else
            {
                Debug.Log("此水果不可召喚");
                cmdCanvasControl.CmdColor(new Color(0.5f, 0.7f, 0f, 0.5f));
            }
            
        }
        else
        {
            Fail();
            Debug.Log("召喚指令錯誤");
        }
    }
    
    void Fail()
    {
        cmdCanvasControl.CmdColor(new Color(1f, 0f, 0f, 0.5f));
    }
}
