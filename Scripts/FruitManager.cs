using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    public float growPosShift = 1f;
    public float playerChange_Delay = 0.2f;
    public static int occupyLimit = 5;
    public int occupy = 0;
    public GameObject[] fruitmanPrefab;
    public UIBucket UI_Bucket;
    public SkillModeControl skillModeControl;

    private int sortOrder = 0;
    private GameObject player;
    private GameObject firstOne;
    private GameObject lastOne;
    private int oriFirstID;
    private FruitmanBase firstFruitBase = null;
    //private PlayerControl playerControl;
    private PlayerOutlook playerOutlook;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //playerControl = player.GetComponent<PlayerControl>();
        playerOutlook = player.GetComponent<PlayerOutlook>();

        firstOne = lastOne = player;
    }

    // Update is called once per frame
    void Update()
    {
        //測試用
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            
            if (firstOne != player)
            {
                firstFruitBase.FressLoss(1000);
            }
        }
    }

    public bool Grow(int id)
    {
        //-----------設定水果籃位----------------

        occupy += FruitmanData.InfoList[id].occupy;
      
        if (occupy > occupyLimit)
        {
            occupy -= FruitmanData.InfoList[id].occupy;
            return false; //回傳false表示超過上限
        }

        UI_Bucket.SetOccupyNum(occupy);

        //-----------設定生成並跟隨----------------

        Vector3 growPosition = lastOne.transform.position;

        if (lastOne.name != "Player")
        {
            if (lastOne.GetComponent<FruitmanBase>().facingRight) growPosition.x -= growPosShift;
            else growPosition.x += growPosShift;
        }

        GameObject newFruitman = (Instantiate(fruitmanPrefab[id], growPosition, Quaternion.identity));
        newFruitman.transform.parent = this.transform;

        if (lastOne.name != "Player")
        {
            lastOne.GetComponent<FruitmanBase>().SetNextOne(newFruitman);
        }
        newFruitman.GetComponent<FruitmanBase>().SetTarget(lastOne);
        lastOne = newFruitman;

        newFruitman.GetComponent<SpriteRenderer>().sortingOrder = sortOrder;
        sortOrder -= 1;

        if (firstOne == player)
        {
            firstOne = newFruitman;
            firstFruitBase = firstOne.GetComponent<FruitmanBase>();

            FirstOneChange(id);
        }

        return true;
    }

    public void FirstOneDie(int id)
    {
        if (firstFruitBase.nextOne != null)
        {
            firstOne = firstFruitBase.nextOne;
            firstFruitBase = firstOne.GetComponent<FruitmanBase>();

            firstFruitBase.followTarget = player;

            FirstOneChange(firstFruitBase.ID);
        }
        else
        {
            firstOne = lastOne = player;

            FirstOneChange(0);
        }

        occupy -= FruitmanData.InfoList[id].occupy;
        UI_Bucket.SetOccupyNum(occupy);
    }

    void FirstOneChange(int firstID)
    {
        if (firstID == oriFirstID) return;

        if (firstID == 0)
        {
            playerOutlook.OutlookChange(0);
            skillModeControl.SetSkillMode(0);
            oriFirstID = 0;

            return;
        }
        else
        {
            playerOutlook.OutlookChange(firstID);
            skillModeControl.SetSkillMode(firstID);
            oriFirstID = firstID;
        }
    }
}