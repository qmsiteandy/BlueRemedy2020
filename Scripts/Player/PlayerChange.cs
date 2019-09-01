using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChange : MonoBehaviour {

    [Header("三態相關")]
    public GameObject[] Oka_form = { null, null, null };
    public int form_now = 1;
    private int form_index;
    private bool isWheelShow = false;

    public GameObject smokeParticke;
    public CameraControl cameraControl;
    private PlayerControl playerControl;
    private PlayerWheel playerWheel;


    // Use this for initialization
    void Start () {
        Oka_form[0] = this.transform.GetChild(0).gameObject;
        Oka_form[1] = this.transform.GetChild(1).gameObject;
        Oka_form[2] = this.transform.GetChild(2).gameObject;

        playerControl = Oka_form[1].GetComponent<PlayerControl>();
        playerWheel = transform.GetChild(3).GetComponent<PlayerWheel>();
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetButtonDown("Change") && !isWheelShow)
        {
            form_index = form_now; playerControl.allCanDo = false;
            playerWheel.WheelShow(); isWheelShow = true;
        }
        if (Input.GetButton("Change") && isWheelShow)
        {
            if (!playerWheel.isSpinFinish) return;

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                form_index += 1; if (form_index > 2) form_index = 0; playerWheel.WheelSpinRight(false);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                form_index -= 1; if (form_index < 0) form_index = 2; playerWheel.WheelSpinRight(true);
            }
        }
        if (Input.GetButtonUp("Change") && isWheelShow)
        {
            playerControl.allCanDo = true;

            if (form_index != form_now)
            {
                GameObject particle = Instantiate(smokeParticke, Oka_form[form_now].transform.position, Oka_form[form_now].transform.rotation);
                Destroy(particle, 1.2f);

                ChangeForm(form_index);
            }

            playerWheel.WheelDisappear(); isWheelShow = false;
        }
    }

    void ChangeForm(int new_index)
    {
        Oka_form[form_now].SetActive(false);

        Oka_form[new_index].SetActive(true);
        Transform newForm_trans = Oka_form[new_index].GetComponent<Transform>();
        newForm_trans.position = Oka_form[form_now].transform.position;
        //StartCoroutine(PosLerp(newForm_trans, Oka_form[form_now].transform.position));



        newForm_trans.localScale = Oka_form[form_now].transform.localScale;

        PlayerControl newPlayerControl = Oka_form[new_index].GetComponent<PlayerControl>();
        newPlayerControl.facingRight = playerControl.facingRight;

        form_now = new_index;
        playerControl = newPlayerControl;

        cameraControl.target = Oka_form[form_now].transform;
    }

    /*IEnumerator PosLerp(Transform newForm_trans,Vector3 aimPos)
    {
        int segment = 10;
        Vector2 offset = (aimPos - newForm_trans.position) / segment;
        for(int i = 0;i< segment; i++)
        {
            newForm_trans.position = new Vector3(offset.x, offset.y, 0f);
            yield return null;
        }
    }*/
}
