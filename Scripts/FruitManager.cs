using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{

    public float growPosShift = 1f;
    public int countLimit = 5;
    public int count = 0;
    public GameObject[] fruitmanPrefab;
 
    private int sortOrder = 0;
    private GameObject player;
    private GameObject firstOne = null;
    private GameObject lastOne = null;
    private FruitmanBase firstFruitBase = null;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lastOne = player;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            //測試用
            if (firstOne != null)
            {
                firstFruitBase.FressLoss(1000);
            }
        }
    }

    public void Grow(int id)
    {
        if (count > countLimit)
        {
            print("超過召喚上限");
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

        if (firstOne == null)
        {
            firstOne = newFruitman;
            firstFruitBase = firstOne.GetComponent<FruitmanBase>();
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
            lastOne = player;
        }

        count--;
    }
}