using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour {

    public Text ID;
    public Text Name;
    public Text Season;
    public Image FruitImg;
    public Sprite[] fruitLook;
    public RectTransform[] cmd = { null, null, null, null };
    public Text Occupy;
    public Text Fresh;
    public Text AttackText;
    public Text DefenceText;
    public Text HealText;
    public BehaviourManager behaviourManager;

    private GameObject bookBack;
    private GameObject bookInfo;
    private Image leftArrow, rightArrow;
    private bool isOpen = false;
    private bool isChanging = false;
    private int pageNow = 1;
    private int pageMax;
    

    // Use this for initialization
    void Start () {
        bookBack = transform.GetChild(0).gameObject;
        bookInfo = transform.GetChild(1).gameObject;
        leftArrow = bookBack.transform.GetChild(0).gameObject.GetComponent<Image>();
        rightArrow = bookBack.transform.GetChild(1).gameObject.GetComponent<Image>();

        pageNow = 1;
        pageMax = FruitmanData.InfoList.Length - 1;

        SetBookInfo(pageNow);

        bookBack.SetActive(false);
        bookInfo.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Book"))
        {
            isOpen = !isOpen;
            bookBack.SetActive(isOpen);
            bookInfo.SetActive(isOpen);

            behaviourManager.BehaviourPause(isOpen);
        }

        float yInput = Input.GetAxisRaw("Horizontal");

        if (isOpen && !isChanging)
        {
            if (yInput > 0)
            {
                pageNow += 1;
                isChanging = true;
            }
            else if(yInput < 0)
            {
                pageNow -= 1;
                isChanging = true;
            }

            pageNow = Mathf.Clamp(pageNow, 1, pageMax);
            SetBookInfo(pageNow);

            if (pageNow == 1)
            {
                leftArrow.enabled = false;
                rightArrow.enabled = true;
            }
            else if (pageNow == pageMax)
            {
                leftArrow.enabled = true;
                rightArrow.enabled = false;
            }
            else
            {
                leftArrow.enabled = true;
                rightArrow.enabled = true;
            }
        }
        else if (yInput == 0)
        {
            isChanging = false;
        }
	}

    void SetBookInfo(int id)
    {
        ID.text = "ID：" + id;
        Name.text = FruitmanData.InfoList[id].name;

        switch (FruitmanData.InfoList[id].season)
        {
            case 1: Season.text = "spring"; break;
            case 2: Season.text = "summer"; break;
            case 3: Season.text = "fall"; break;
            case 4: Season.text = "winter"; break;
        }

        FruitImg.sprite = fruitLook[id];

        for(int x = 0; x < 4; x++)
        {
            switch (FruitmanData.InfoList[id].cmd[x])
            {
                case 1: cmd[x].localRotation = (Quaternion.Euler(0f, 0f, 0f)); break;
                case 2: cmd[x].localRotation = (Quaternion.Euler(0f, 0f, 180f)); break;
                case 3: cmd[x].localRotation = (Quaternion.Euler(0f, 0f, 90f)); break;
                case 4: cmd[x].localRotation = (Quaternion.Euler(0f, 0f, -90f)); break;
            }
        }

        Occupy.text = "occupy：" + FruitmanData.InfoList[id].occupy;
        Fresh.text = "fresh：" + FruitmanData.InfoList[id].fresh;
        AttackText.text = "" + FruitmanData.InfoList[id].attack;
        DefenceText.text = "" + FruitmanData.InfoList[id].defence;
        HealText.text = "" + FruitmanData.InfoList[id].heal;

    }
}
