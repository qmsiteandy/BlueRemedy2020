using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChange : MonoBehaviour {

    [Header("三態相關")]
    public GameObject[] Oka_form = { null, null, null };
    private int form_index;
    private bool transforming = false;
    private bool wheelShow = false;

    /*public GameObject smokeParticke;*/
    private PlayerControl playerControl;
    private PlayerWheel playerWheel;
    private Transform wheelTrans;

    private bool isChangeAxisDown = false;


    // Use this for initialization
    void Start () {
        Oka_form[0] = this.transform.GetChild(0).gameObject;
        Oka_form[1] = this.transform.GetChild(1).gameObject;
        Oka_form[2] = this.transform.GetChild(2).gameObject;

        playerControl = GetComponent<PlayerControl>();
        wheelTrans = transform.Find("Wheel").GetComponent<Transform>();
        playerWheel = wheelTrans.GetComponent<PlayerWheel>();
    }
	
    

	// Update is called once per frame
	void Update()
    {
        //bool getKey_Change;
        //if (PlayerStatus.isKeyboardInput()) getKey_Change = Input.GetButton("Change");
        //else getKey_Change = Input.GetAxis("XBOX_Change") > 0f;

        if ((PlayerStatus.isKeyboardInput() ? Input.GetButtonDown("Change") : Input.GetAxis("XBOX_Change") > 0f) && !wheelShow && PlayerStatus.canChange)
        {
            //form_index = PlayerControl.OkaID_Now;
            PlayerStatus.isChanging = true;
            playerWheel.WheelShow(); wheelShow = true;
        }
        else if ((PlayerStatus.isKeyboardInput() ? Input.GetButton("Change") : Input.GetAxis("XBOX_Change") > 0f) && !transforming && wheelShow)
        {
            if (PlayerStatus.isKeyboardInput() ? Input.GetKeyDown(KeyCode.LeftArrow) : Input.GetKeyDown(KeyCode.JoystickButton2))
            {
                if (PlayerControl.OkaID_Now == 0) { playerWheel.LightFlash(0); return; }
                form_index = 0;
                ChangeForm(form_index);
                playerWheel.WheelIndexSelect(0);
                playerWheel.WheelDisappear(); wheelShow = false;
            }
            else if (PlayerStatus.isKeyboardInput() ? Input.GetKeyDown(KeyCode.UpArrow) : Input.GetKeyDown(KeyCode.JoystickButton3))
            {
                if (PlayerControl.OkaID_Now == 1) { playerWheel.LightFlash(1); return; }
                form_index = 1;
                ChangeForm(form_index);
                playerWheel.WheelIndexSelect(1);
                playerWheel.WheelDisappear(); wheelShow = false;
            }
            else if (PlayerStatus.isKeyboardInput() ? Input.GetKeyDown(KeyCode.RightArrow) : Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                if (PlayerControl.OkaID_Now == 2) { playerWheel.LightFlash(2); return; }
                form_index = 2;
                ChangeForm(form_index);
                playerWheel.WheelIndexSelect(2);
                playerWheel.WheelDisappear(); wheelShow = false;
            }
        }
        if ((PlayerStatus.isKeyboardInput() ? Input.GetButtonUp("Change") : Input.GetAxis("XBOX_Change") == 0f) && wheelShow)
        {
            playerWheel.WheelDisappear(); wheelShow = false;
            PlayerStatus.isChanging = false;
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
        PlayerStatus.isChanging = true;
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
