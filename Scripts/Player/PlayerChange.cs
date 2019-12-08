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
	void Update ()
    {
        if (Input.GetButtonDown("Change") && !wheelShow && PlayerControl.canMove)
        {
            form_index = PlayerControl.OkaID_Now;
            PlayerControl.canMove = false;
            playerWheel.WheelShow(); wheelShow = true;
        }
        else if (Input.GetButton("Change") && !transforming && wheelShow)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (PlayerControl.OkaID_Now == 0) { playerWheel.LightFlash(0); return; }

                form_index = 0;
                ChangeForm(form_index);
                playerWheel.WheelIndexSelect(0);
                playerWheel.WheelDisappear(); wheelShow = false;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (PlayerControl.OkaID_Now == 1) { playerWheel.LightFlash(1); return; }

                form_index = 1;
                ChangeForm(form_index);
                playerWheel.WheelIndexSelect(1);
                playerWheel.WheelDisappear(); wheelShow = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (PlayerControl.OkaID_Now == 2) { playerWheel.LightFlash(2); return; }

                form_index = 2;
                ChangeForm(form_index);
                playerWheel.WheelIndexSelect(2);
                playerWheel.WheelDisappear(); wheelShow = false;
            } 
        }
        else if (Input.GetButtonUp("Change") && wheelShow)
        {
            playerWheel.WheelDisappear(); wheelShow = false;
            PlayerControl.canMove = true;
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
        PlayerControl.canMove = true;
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
