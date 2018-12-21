using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float cmdTimeLimit = 2f;

    private FruitManager fruitManager;
    private PlayerControl playerControl;
    private int[] cmdIn = { 0, 0, 0, 0 };
    private int index = 0;
    private float elapsed = 0f;
    private bool isInputting = false;

    private Canvas cmdCanvas;
    private Image[] cmdImg;
    private RectTransform[] cmdRect;


    void Start () {
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        fruitManager = GetComponent<FruitManager>();

        cmdCanvas = gameObject.transform.GetChild(0).gameObject.GetComponent<Canvas>();
        cmdCanvas.enabled = false;

        cmdRect[0] = gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>();
        cmdRect[1] = gameObject.transform.GetChild(0).GetChild(2).gameObject.GetComponent<RectTransform>();
        cmdRect[2] = gameObject.transform.GetChild(0).GetChild(3).gameObject.GetComponent<RectTransform>();
        cmdRect[3] = gameObject.transform.GetChild(0).GetChild(4).gameObject.GetComponent<RectTransform>();
        cmdImg[0] = gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>();
        cmdImg[1] = gameObject.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Image>();
        cmdImg[2] = gameObject.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Image>();
        cmdImg[3] = gameObject.transform.GetChild(0).GetChild(4).gameObject.GetComponent<Image>();
        cmdImg[0].enabled = false;
        cmdImg[1].enabled = false;
        cmdImg[2].enabled = false;
        cmdImg[3].enabled = false;
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isInputting = true;
            playerControl.allCanDo = false;

            cmdCanvas.enabled=true;
        }

        if (isInputting)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                cmdIn[index] = 1;

                cmdRect[index].Rotate(new Vector3(0, 0, 0));
                cmdImg[index].enabled = true;

                index++;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                cmdIn[index] = 2;

                cmdRect[index].Rotate(new Vector3(0, 0, 180));
                cmdImg[index].enabled = true;

                index++;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                cmdIn[index] = 3;

                cmdRect[index].Rotate(new Vector3(0, 0, 90));
                cmdImg[index].enabled = true;

                index++;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                cmdIn[index] = 4;

                cmdRect[index].Rotate(new Vector3(0, 0, -90));
                cmdImg[index].enabled = true;

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
            fruitManager.Grow(matchId);
            CanvasReturn();
        }
        else
        {
            Fail();
            CanvasReturn();
        }
    }
    

    void Fail()
    {
        
    }

    void CanvasReturn()
    {
        cmdCanvas.enabled = false;
        cmdImg[0].enabled = false;
        cmdImg[1].enabled = false;
        cmdImg[2].enabled = false;
        cmdImg[3].enabled = false;
    }
}
