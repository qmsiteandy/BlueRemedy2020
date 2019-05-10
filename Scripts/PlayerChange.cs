using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChange : MonoBehaviour {

    [Header("三態相關")]
    public GameObject[] Oka_form = { null, null, null };
    private int form_now = 1;
    private int form_index;

    [Header("轉輪物件")]
    public GameObject wheel;
    Transform lightArea_trans;

    public GameObject smokeParticke;
    public CameraControl cameraControl;
    private PlayerControl playerControl;


    // Use this for initialization
    void Start () {
        Oka_form[0] = this.transform.GetChild(0).gameObject;
        Oka_form[1] = this.transform.GetChild(1).gameObject;
        Oka_form[2] = this.transform.GetChild(2).gameObject;

        playerControl = Oka_form[1].GetComponent<PlayerControl>();

        lightArea_trans = wheel.transform.GetChild(0).transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        wheel.transform.position = Oka_form[form_now].transform.position;

        if (Input.GetButtonDown("Change"))
        {
            form_index = form_now; playerControl.allCanDo = false;
            wheel.SetActive(true);
        }
        if (Input.GetButton("Change"))
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                form_index = 0; lightArea_trans.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                form_index = 1; lightArea_trans.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                form_index = 2; lightArea_trans.rotation = Quaternion.Euler(0f, 0f, -90f);
            }
        }
        if (Input.GetButtonUp("Change"))
        {
            playerControl.allCanDo = true;

            if (form_index != form_now)
            {
                GameObject particle = Instantiate(smokeParticke, Oka_form[form_now].transform.position, Oka_form[form_now].transform.rotation);
                Destroy(particle, 1.2f);

                ChangeForm(form_index);
            }

            wheel.SetActive(false);
        }
    }

    void ChangeForm(int form_index)
    {
        Oka_form[form_now].SetActive(false);

        Transform newForm_trans = Oka_form[form_index].GetComponent<Transform>();
        newForm_trans.position = Oka_form[form_now].transform.position;
        newForm_trans.localScale = Oka_form[form_now].transform.localScale;
        Oka_form[form_index].GetComponent<PlayerControl>().facingRight = Oka_form[form_now].GetComponent<PlayerControl>().facingRight;
        playerControl = Oka_form[form_index].GetComponent<PlayerControl>();

        Oka_form[form_index].SetActive(true);
        form_now = form_index;

        cameraControl.target = Oka_form[form_now].transform;
    }
}
