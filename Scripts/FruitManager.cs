using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{

    public float growPosShift = 1f;
    public float playerChange_Delay = 0.2f;
    public int countLimit = 5;
    public int count = 0;
    public GameObject[] fruitmanPrefab;
 
    private int sortOrder = 0;
    private GameObject player;
    private GameObject firstOne;
    private GameObject lastOne;
    private FruitmanBase firstFruitBase = null;
    private PlayerControl playerControl;
    private PlayerOutlook playerOutlook;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        playerControl = player.GetComponent<PlayerControl>();
        playerOutlook = player.GetComponent<PlayerOutlook>();

        firstOne = lastOne = player;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            //測試用
            if (firstOne != player)
            {
                firstFruitBase.FressLoss(1000);
            }
        }
    }

    public void Grow(int id)
    {
        if (count > countLimit)
        {
            return;
        }

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

            FirstOneChange(firstOne.name);
        }

        count++;
    }

    public void FirstOneDie()
    {
        if (firstFruitBase.nextOne != null)
        {
            firstOne = firstFruitBase.nextOne;
            firstFruitBase = firstOne.GetComponent<FruitmanBase>();

            firstFruitBase.followTarget = player;
        }
        else
        {
            firstOne = lastOne = player;
        }

        FirstOneChange(firstOne.name);

        count--;
    }

    void FirstOneChange(string firstName)
    {
        if (firstName == "Player")
        {
            playerOutlook.OutlookChange(0);

            return;
        }

        for (int id = 1; id < fruitmanPrefab.Length; id++)
        {
            if (firstName == (fruitmanPrefab[id].name + "(Clone)")) 
            {
                Debug.Log("FirstOneID = " + id);

                playerOutlook.OutlookChange(id);

                return;
            }
        }
    }
}