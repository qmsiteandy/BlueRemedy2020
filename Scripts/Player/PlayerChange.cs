using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChange : MonoBehaviour {

    [Header("三態相關")]
    public GameObject[] Oka_form = { null, null, null };
    private int form_index;
    private bool transforming = false;

    /*public GameObject smokeParticke;*/
    private PlayerControl playerControl;
    private PlayerWheel playerWheel;
    private RectTransform wheelTrans;

    private bool isChangeAxisDown = false;


    // Use this for initialization
    void Start () {
        Oka_form[0] = this.transform.GetChild(0).gameObject;
        Oka_form[1] = this.transform.GetChild(1).gameObject;
        Oka_form[2] = this.transform.GetChild(2).gameObject;

        playerControl = GetComponent<PlayerControl>();
        wheelTrans = transform.Find("WheelCanvas").GetComponent<RectTransform>();
        playerWheel = wheelTrans.GetComponent<PlayerWheel>();
    }
	
    

	// Update is called once per frame
	void Update()
    {
        //bool getKey_Change;
        //if (PlayerStatus.isKeyboardInput()) getKey_Change = Input.GetButton("Change");
        //else getKey_Change = Input.GetAxis("XBOX_Change") > 0f;

        if ((PlayerStatus.Get_isKeyboard() ? Input.GetButtonDown("Change") : Input.GetAxis("XBOX_Change") > 0f) && !playerWheel.Get_wheelShow() && PlayerStatus.canChange)
        {
            form_index = PlayerControl.OkaID_Now;
            PlayerStatus.isChanging = true;
            playerWheel.WheelShow();
        }
        else if ((PlayerStatus.Get_isKeyboard() ? Input.GetButton("Change") : Input.GetAxis("XBOX_Change") > 0f) && !transforming && playerWheel.Get_wheelShow())
        {
            //選擇"冰"
            if (PlayerStatus.Get_isKeyboard() ? Input.GetKeyDown(KeyCode.LeftArrow) : Input.GetAxis("XBOX_Horizontal") < -0.7f)
            {
                form_index = 0;
                playerWheel.WheelIndexSelect(0);
            }
            //選擇"水"
            else if (PlayerStatus.Get_isKeyboard() ? Input.GetKeyDown(KeyCode.UpArrow) : Input.GetAxis("XBOX_Vertical") < -0.7f)
            {
                form_index = 1;
                playerWheel.WheelIndexSelect(1);
            }
            //選擇"雲"
            else if (PlayerStatus.Get_isKeyboard() ? Input.GetKeyDown(KeyCode.RightArrow) : Input.GetAxis("XBOX_Horizontal") > 0.7f)
            {               
                form_index = 2;                
                playerWheel.WheelIndexSelect(2);
            }
        }
        if ((PlayerStatus.Get_isKeyboard() ? Input.GetButtonUp("Change") : Input.GetAxis("XBOX_Change") == 0f) && playerWheel.Get_wheelShow())
        {

            if (form_index != PlayerControl.OkaID_Now) ChangeForm(form_index);
            else PlayerStatus.isChanging = false;

            playerWheel.WheelDisappear();            
        }
    }

    void ChangeForm(int new_index)
    {
        transforming = true;

        int x = new_index - PlayerControl.OkaID_Now;
        //change next
        if (x == 1 || x == -2) Oka_form[PlayerControl.OkaID_Now].GetComponent<Skill_Base>().ChangeStart(true);
        //change previous
        else if (x == -1 || x == 2) Oka_form[PlayerControl.OkaID_Now].GetComponent<Skill_Base>().ChangeStart(false);

        playerControl.TrnasformReset();
    }

    //由Skill_Base呼叫
    public void ChangeFinish()
    {
        Oka_form[PlayerControl.OkaID_Now].SetActive(false);
        Oka_form[form_index].SetActive(true);
        Oka_form[form_index].GetComponent<Skill_Base>().TransformReset();

        PlayerControl.OkaID_Now = form_index;

        transforming = false;
        PlayerStatus.isChanging = false;
        PlayerStatus.isInInteractTrigger = false;
    }

    public void WheelUI_Flip()
    {
        //取得目前物件的規格
        Vector3 theScale = wheelTrans.localScale;
        //使規格的水平方向相反
        theScale.x *= -1;
        //套用翻面後的規格
        wheelTrans.localScale = theScale;
    }
}
