using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour {

    public float smoothSpeed = 3f;
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
    private CanvasGroup book_CanvasGroup, info_CanvasGroup;
    [Range(0f, 1f)]private float book_alpha = 0f;
    private Image leftArrow, rightArrow;
    private bool isOpen = false;
    private bool isAlphaChanging = false;
    private bool isInfoChanging = false;
    private int pageNow = 1;
    private int pageMax;
    

    // Use this for initialization
    void Start () {
        bookBack = transform.GetChild(0).gameObject;
        bookInfo = transform.GetChild(1).gameObject;
        leftArrow = bookBack.transform.GetChild(0).gameObject.GetComponent<Image>();
        rightArrow = bookBack.transform.GetChild(1).gameObject.GetComponent<Image>();

        book_CanvasGroup = GetComponent<CanvasGroup>();
        book_CanvasGroup.alpha = book_alpha;
        info_CanvasGroup = bookInfo.GetComponent<CanvasGroup>();

        pageNow = 1;
        pageMax = FruitmanData.InfoList.Length - 1;

        SetBookInfo(pageNow);

        //bookBack.SetActive(false);
        //bookInfo.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Book") && !isAlphaChanging)
        {
            isOpen = !isOpen;

            StartCoroutine(BookShow(isOpen));
            isAlphaChanging = true;

            behaviourManager.BehaviourPause(isOpen);
        }

        float yInput = Input.GetAxisRaw("Horizontal");

        if (isOpen && !isInfoChanging)
        {
            if (yInput > 0)
            {
                pageNow += 1;
                isInfoChanging = true;
            }
            else if(yInput < 0)
            {
                pageNow -= 1;
                isInfoChanging = true;
            }


            if (pageNow < 1 || pageNow > pageMax)
            {
                pageNow = Mathf.Clamp(pageNow, 1, pageMax);
                isInfoChanging = false;
            }
            if (isInfoChanging) StartCoroutine(SetBookInfo(pageNow));


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
            isInfoChanging = false;
        }
	}

    IEnumerator SetBookInfo(int id)
    {
        float info_alpha = 1f;

        while (info_alpha > 0f)
        {
            info_alpha -= smoothSpeed * 1.5f * Time.deltaTime;
            info_CanvasGroup.alpha = info_alpha;

            yield return null;
        }

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

        while (info_alpha < 1f)
        {
            info_alpha += smoothSpeed * 1.5f * Time.deltaTime;
            info_CanvasGroup.alpha = info_alpha;

            yield return null;
        }

    }

    IEnumerator BookShow(bool open)
    {
        if (open)
        {
            while (book_alpha < 1f)
            {
                book_alpha += smoothSpeed * Time.deltaTime;
                book_CanvasGroup.alpha = book_alpha;

                yield return null;
            }
        }
        else
        {
            while (book_alpha > 0f)
            {
                book_alpha -= smoothSpeed * Time.deltaTime;
                book_CanvasGroup.alpha = book_alpha;

                yield return null;
            }
        }
        isAlphaChanging = false;
    }
}
