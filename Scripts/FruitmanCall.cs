using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitmanCall : MonoBehaviour {

    //----------召喚相關資訊--------------------

    //水果指令
    private int[][] fruitCmd = {
        new int[] {1,2,3,4},
        new int[] {2,3,4,1},
        new int[] {3,4,1,2}
    };
    //可否召喚該水果
    public bool[] canCall = {
        true,
        false,
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
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                cmdIn[index] = 1;
                cmdCanvasControl.CmdShow(index, 0);

                index++;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                cmdIn[index] = 2;
                cmdCanvasControl.CmdShow(index, 180);

                index++;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                cmdIn[index] = 3;
                cmdCanvasControl.CmdShow(index, 90);

                index++;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
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

        for (int id = 0; id < fruitCmd.Length; id++)
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
                fruitManager.Grow(matchId);

                cmdCanvasControl.CmdSuccess();
            }
            else
            {
                Debug.Log("此水果不可召喚");
                Fail();
            }
            
        }
        else
        {
            Fail();
        }
    }
    
    void Fail()
    {
        cmdCanvasControl.CmdFail();
    }
}
