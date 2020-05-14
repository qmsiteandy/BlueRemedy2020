using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelDoor : MonoBehaviour {

    [Header("門功能")]
    public string toSceneName;
    public int doorID;

    private class c_door
    {
        public Vector3 doorPos = Vector3.zero;
        public bool isDoorOpen = false;
    }
    private static c_door[] doorList = new c_door[6];

    private static int lastEnterDoorId = 999;


    [Header("輸入&轉換")]
    private float enterInputTime = 0f;
    private bool isChange = false;
    private bool isHoldingInput = false;
    
    
    [Header("門閃亮FX")]
    private ParticleSystem doorShineFx;
    private float particleStartSpeed;
    private float particleStartAmount;

    [Header("UI顯現")]
    private float fadeUpTime = 0.2f;
    private float fadeDownTime = 0.25f;
    private CanvasGroup canvasGroup;

    [Header("開門")]
    private GameObject vcam;
    public GameObject doorClose, doorOpen;

    // Use this for initialization
    void Awake ()
    {
        //---門的ID
        for(int i = 0;i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)) == toSceneName)
            {
                doorID = i - 2; //Scene編號0、1分別為Start_Scene及Level_Room
                break;
            }
        }

        //---初始化doorList
        if (doorList[doorID] == null)
        {
            doorList[doorID] = new c_door();
            doorList[doorID].doorPos = this.transform.position;
            if (doorID == 0) doorList[doorID].isDoorOpen = true;
        }
        
        //---門的樣式初始
        doorClose = transform.Find("door_IMG/door_close").gameObject;
        doorOpen = transform.Find("door_IMG/door_open").gameObject;
        doorClose.GetComponent<SpriteRenderer>().sortingOrder = doorOpen.GetComponent<SpriteRenderer>().sortingOrder - 1;

        if (doorList[doorID].isDoorOpen == true) //如果早已打開
        {
            transform.Find("door_IMG").GetComponent<Animator>().SetTrigger("open");
        }

        //---門閃亮FX初始
        doorShineFx = this.transform.Find("doorShineFx").GetComponent<ParticleSystem>();
        particleStartSpeed = doorShineFx.startSpeed;
        particleStartAmount = doorShineFx.emissionRate;


        //----UI顯現
        canvasGroup = transform.Find("canvas").Find("InputNote_group").GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        //---開門相關
        vcam = this.transform.Find("CM vcam").gameObject;
        vcam.SetActive(false);
    }

    void Update()
    {
        if (doorList[doorID].isDoorOpen == false)
        {
            if (doorID <= LevelData.get_LevelRecord()) OpenThisDoor();    
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && doorList[doorID].isDoorOpen) canvasGroup.DOFade(1f, fadeUpTime);
    }
    void OnTriggerStay2D(Collider2D other)
    { 
        if (other.tag == "Player" && isChange == false && doorList[doorID].isDoorOpen)
        {
            if (Input.GetButton("Submit") || Input.GetKey(KeyCode.Space))
            {
                PlayerStatus.isSkilling = true;
                isHoldingInput = true;

                enterInputTime += Time.deltaTime;

                if (doorShineFx.isPlaying == false) doorShineFx.Play();
                doorShineFx.startSpeed = particleStartSpeed * (enterInputTime * 1.5f);
                doorShineFx.emissionRate = particleStartAmount * (enterInputTime * 1.2f);

                if (enterInputTime > 1.5f)
                {
                    doorShineFx.Emit(25);
                    doorShineFx.Stop();

                    ChangeScene();
                    lastEnterDoorId = this.doorID;
                }
            }
            if ((!Input.GetButton("Submit") && !Input.GetKey(KeyCode.Space)) && isHoldingInput)
            {
                PlayerStatus.isSkilling = false;
                isHoldingInput = false;

                enterInputTime = 0f;

                if (doorShineFx.isPlaying == true) doorShineFx.Stop();
                doorShineFx.startSpeed = particleStartSpeed;
                doorShineFx.emissionRate = particleStartAmount;
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && doorList[doorID].isDoorOpen) canvasGroup.DOFade(0f, fadeDownTime);

        PlayerStatus.isSkilling = false;
        enterInputTime = 0f;

        if (doorShineFx.isPlaying == true) doorShineFx.Stop();
        doorShineFx.startSpeed = particleStartSpeed;
        doorShineFx.emissionRate = particleStartAmount;
    }

    public void ChangeScene()
    {
        isChange = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().GoToScene(toSceneName);
    }

    public void OpenThisDoor()
    {
        StartCoroutine(cor_OpenThisDoor());
    }
    IEnumerator cor_OpenThisDoor()
    {
        yield return new WaitForSeconds(1f);    //等待轉場後畫面亮起

        PlayerStatus.canControl = false;
        vcam.SetActive(true);

        yield return new WaitForSeconds(1f);

        this.transform.Find("door_IMG").GetComponent<Animator>().SetTrigger("open");
        doorList[doorID].isDoorOpen = true;

        yield return new WaitForSeconds(1f);

        PlayerStatus.canControl = true;
        vcam.SetActive(false);
    }

    public static Vector3 get_lastEnterDoorPos()
    {
        Vector3 pos;

        if (lastEnterDoorId < doorList.Length)
        {
            pos = doorList[lastEnterDoorId].doorPos;
        }
        else pos = Vector3.zero;

        return (pos);
    }
}
