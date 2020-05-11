using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Gas : Skill_Base
{
    [Header("Special Skill")]
    private int specialCost_max = 15;
    private float specialInput_holdTime = 0f;
    private bool btnHolding = false;
    public GameObject tornadoPrefab;
    private GameObject tornadoObj;
    private Special_g_tornado special_g_tornado;
    private Transform tornadoInitPoint; 
    private enum SpecialSkill_State { none, charging, holding }
    private SpecialSkill_State specialSkill_state = SpecialSkill_State.none;
    private float throwAngle = 90f;
    private Vector2 throwAngleLimit = new Vector2(30f, 150f);


    void Awake()
    {
        tornadoInitPoint = transform.Find("specialTornado_initPoint").transform;
    }
    void Start()
    {
        BaseStart();
    }

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();

        if (Input.GetButtonDown("Attack")) if (specialSkill_state == SpecialSkill_State.none) specialSkill_state = SpecialSkill_State.charging;

        if (Input.GetButton("Attack") && PlayerStatus.canSkill && specialSkill_state != SpecialSkill_State.none && normalattack_input <= 1)
        {
            specialInput_holdTime += Time.deltaTime;

            if (specialInput_holdTime >= 0.3f)
            {
                SpecialAttacking(true);

                switch (specialSkill_state)
                {
                    //開始charging
                    case SpecialSkill_State.charging:

                        animator.SetTrigger("specialSkill_charging");

                        specialSkill_state = SpecialSkill_State.holding;

                        break;

                    //charging中
                    case SpecialSkill_State.holding:

                        if (tornadoObj == null)
                        {
                            tornadoObj = Instantiate(tornadoPrefab, tornadoInitPoint.position, Quaternion.identity);
                            special_g_tornado = tornadoObj.GetComponent<Special_g_tornado>();
                            special_g_tornado.SetSpeed(PlayerControl.facingRight ? 3f : -3f);

                            throwAngle = 90f;
                            special_g_tornado.SetAngle(throwAngle);
                        }
                        else
                        {
                            AngleInput();
                            special_g_tornado.SetAngle(throwAngle);

                            //playerEnergy.ModifyWaterEnergy(-(int)(specialCost_max));
                        }

                        break;
                }
            }
        }
        //釋放
        if (Input.GetButtonUp("Attack"))
        {
            specialInput_holdTime = 0f;

            if (specialSkill_state == SpecialSkill_State.holding)
            {
                this.TornadoBreak();
            }
        }
    }

    void AngleInput()
    {
        float xInput = PlayerStatus.Get_isKeyboard() ? Input.GetAxis("Horizontal") : Input.GetAxis("XBOX_Horizontal");
        float yInput = PlayerStatus.Get_isKeyboard() ? Input.GetAxis("Vertical") : Input.GetAxis("XBOX_Vertical");
        if (yInput < 0f) yInput = 0f;

        if(xInput!=0f || yInput != 0f)
        {
            throwAngle = Mathf.Atan2(yInput, xInput);
            throwAngle = throwAngle / Mathf.PI * 180f;

            //當角度小於最小限制時
            if (throwAngle < throwAngleLimit.x || throwAngle > 270f) { throwAngle = throwAngleLimit.x; }
            //當角度大於最大限制時
            else if (throwAngle > throwAngleLimit.y && throwAngle <= 270f) { throwAngle = throwAngleLimit.y; }
        }
    }

    void TornadoBreak()
    {
        specialSkill_state = SpecialSkill_State.none;
        btnHolding = false;

        animator.SetTrigger("specialSkill_release");

        special_g_tornado.Release();
    }

    void SpecialAttacking(bool truefalse)
    {
        attacking = truefalse;
        PlayerStatus.isGasSpecialSkilling = truefalse;
    }

    void SpecialSkillAnimOver()
    {
        SpecialAttacking(false);
    }
}