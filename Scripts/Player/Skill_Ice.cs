using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Ice : Skill_Base {

    [Header("Special Skill")]
    public GameObject icePrefab;
    private GameObject iceObj;
    private int iceObj_layer;
    private float specialInput_holdTime = 0f;
    private float iceSize;
    private float iceMaxSize = 0.95f;
    private Transform iceInitPoint;
    private enum SpecialSkill_State {none, charging, throwout}
    private SpecialSkill_State specialSkill_state = SpecialSkill_State.none;
    private Transform throwIce_arrow;
    private float iceThrowAngle = 0f;
    private float iceThrowForce = 1000f;

    [Header("Normal Skill")]
    public GameObject Attack3_FX;

    void Awake()
    {
        iceInitPoint = transform.Find("specialIce _initPoint").transform;
        throwIce_arrow = transform.Find("throwIce_arrow").transform;
        throwIce_arrow.gameObject.SetActive(false);
    }

    void Start()
    {
        BaseStart();

        iceObj_layer = icePrefab.layer;
    }

    void Update()
    {
        BaseUpdate();

        if (Input.GetButton("Attack") && PlayerStatus.canSkill && normalattack_input <= 1)
        {
            SetAttacking(true);

            specialInput_holdTime += Time.deltaTime;

            if(specialInput_holdTime >= 0.3f)
            {
                switch (specialSkill_state)
                {
                    //開始charging
                    case SpecialSkill_State.none:
                        
                        animator.SetTrigger("specialSkill_charging");

                        specialSkill_state = SpecialSkill_State.charging;
                        
                        break;

                    //charging中
                    case SpecialSkill_State.charging:

                        if (iceObj == null)
                        {
                            iceObj = Instantiate(icePrefab, iceInitPoint.position, Quaternion.identity);
                            iceObj.transform.SetParent(iceInitPoint);

                            iceObj.GetComponent<Rigidbody2D>().isKinematic = true;
                            iceSize = 0.01f;
                            iceObj.transform.localScale = new Vector3(iceSize, iceSize, 1f);

                            iceThrowAngle = PlayerControl.facingRight? 0f: 180f;
                        }
                        else
                        {
                            iceSize += Time.deltaTime * 0.75f;
                            if (iceSize > iceMaxSize) iceSize = iceMaxSize;
                            iceObj.transform.localScale = new Vector3(iceSize, iceSize, 1f);

                            playerControl.rb2d.velocity = Vector2.zero;

                            ThrowIce_AngleInput();
                            SetThrowIce_arrow(iceThrowAngle);
                        }

                        break;
                }
            }
        }
        //釋放
        else if (Input.GetButtonUp("Attack"))
        {
            specialInput_holdTime = 0f;

            if(specialSkill_state == SpecialSkill_State.charging)
            {
                float downThrowRange = 15f;
                if(iceThrowAngle > -90f - downThrowRange && iceThrowAngle < -90f + downThrowRange)
                {
                    animator.SetTrigger("specialSkill_release");    //throwout由release動畫呼叫
                    iceThrowAngle = -90f;
                    iceThrowForce = 500f;

                    playerControl.rb2d.AddForce(Vector2.up * 600f);
                    //StartCoroutine(IceThrow_colDis(0.35f));
                    iceObj.GetComponent<SpecialBigIce>().StartCoroutine(IceThrow_colDis(0.5f));
                }
                else
                {
                    animator.SetTrigger("specialSkill_release");    //throwout由release動畫呼叫
                    iceThrowForce = 1000f;
                }

                specialSkill_state = SpecialSkill_State.none;

                CloseThrowIce_arrow();
            }
        }
    }

    void ThrowIce_AngleInput()
    {
        if (PlayerStatus.Get_isKeyboard())
        {
            float xInput = Input.GetAxis("Horizontal");
            float yInput = Input.GetAxis("Vertical");

            if (Mathf.Abs(xInput) >= 0.1f || Mathf.Abs(yInput) >= 0.1f)
            {
                iceThrowAngle = Mathf.Atan2(yInput, xInput);
                iceThrowAngle = iceThrowAngle / Mathf.PI * 180f;

                //if (iceThrowAngle == -90f) iceThrowAngle = 270f;
                //else if (iceThrowAngle == -45f) iceThrowAngle = 315f;
                //else if (iceThrowAngle == -135f) iceThrowAngle = 225f;
            }
        }
        else
        {
            float xInput = Input.GetAxis("XBOX_Horizontal");
            float yInput = Input.GetAxis("XBOX_Vertical");

            if (Mathf.Abs(xInput) >= 0.1f || Mathf.Abs(yInput) >= 0.1f)
            {
                iceThrowAngle = Mathf.Atan2(-yInput, xInput);
                iceThrowAngle = iceThrowAngle / Mathf.PI * 180f;
            }
        }
    }
    void SetThrowIce_arrow(float toDegree)
    {
        if (throwIce_arrow.gameObject.active == false) throwIce_arrow.gameObject.SetActive(true);

        throwIce_arrow.localScale = new Vector3(PlayerControl.facingRight ? 1f : -1f, 1f, 1f);

        ////鍵盤選角度 指針lerp
        //if(PlayerStatus.Get_isKeyboard() == true)
        //{ 
        //    if (throwIce_arrow.transform.eulerAngles.z >180f && toDegree == 0f) toDegree = 360f;
        //    else if (throwIce_arrow.transform.eulerAngles.z == 0f && toDegree == 270f) throwIce_arrow.transform.eulerAngles = new Vector3(0f, 0f, 359.9f);

        //    Debug.Log(throwIce_arrow.transform.eulerAngles.z + " to " + toDegree);
        //    toDegree = Mathf.Lerp(throwIce_arrow.transform.eulerAngles.z, toDegree, 0.5f);
        //}

        throwIce_arrow.transform.rotation = Quaternion.Euler(0f, 0f, toDegree);
    }
    void CloseThrowIce_arrow()
    {
        if (throwIce_arrow.gameObject.active == true) throwIce_arrow.gameObject.SetActive(false);
    }
    void IceThrowOut()  //由release動畫呼叫
    {
        Vector2 force = new Vector2(Mathf.Cos(iceThrowAngle * (Mathf.PI / 180f)) * iceThrowForce, Mathf.Sin(iceThrowAngle * (Mathf.PI / 180f)) * iceThrowForce);
        iceObj.GetComponent<Rigidbody2D>().isKinematic = false;
        iceObj.GetComponent<Rigidbody2D>().AddForce(force);
        iceObj.GetComponent<SpecialBigIce>().ThrowOut(30f);

        iceObj.transform.SetParent(null);
        iceObj = null;
    }
    void SpecialSkillOver()
    {
        SetAttacking(false);
    }
    IEnumerator IceThrow_colDis(float colDisTime)
    {
        Physics2D.IgnoreLayerCollision(8, iceObj_layer, true);  //SpecialSkillOver時恢復碰撞
        yield return new WaitForSeconds(colDisTime);
        Physics2D.IgnoreLayerCollision(8, iceObj_layer, false);
    }

    void Attack3_FX_Init()
    {
        GameObject FX = Instantiate(Attack3_FX, this.transform.position, Quaternion.identity);
        if (!PlayerControl.facingRight) FX.transform.localScale = new Vector3(-FX.transform.localScale.x, FX.transform.localScale.y, FX.transform.localScale.z);
        Destroy(FX, 5f);
    }
}
