using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Water : Skill_Base
{
    [Header("毛細滲透")]
    private float holePassingSped = 0.07f;
    [HideInInspector] public bool isPassing = false;
    private bool waterdropPassing = false;
    private GameObject whatWaterPassing;
    private Transform waterdropTrans;
    GameObject playerPointLight;

    [Header("Normal Skill")]
    public GameObject Attack3_FX;

    [Header("Special Skill")]
    public GameObject waterCanonPrefab;
    private int specialCost = 20;
    private GameObject waterCanonObj;
    private enum SpecialSkill_State { none, charging, throwout }
    private SpecialSkill_State specialSkill_state = SpecialSkill_State.none;
    private float canonDirectionAngle = 0f;
    private float specialInput_holdTime = 0f;
    private bool isSpitting = false;
    private Coroutine reactionF_routine;

    // Use this for initialization
    void Start ()
    {
        BaseStart();
    }

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();

        //毛細滲透
        if (waterdropPassing)
        {
            playerTrans.position = waterdropTrans.position;
        }

        //Special Skill
        if (Input.GetButton("Attack") && PlayerStatus.canSkill && normalattack_input <= 1)
        {
            specialInput_holdTime += Time.deltaTime;   

            if (specialInput_holdTime >= 0.3f)
            {
                SpecialAttacking(true);

                switch (specialSkill_state)
                {
                    //開始charging
                    case SpecialSkill_State.none:
                        {
                            animator.SetTrigger("specialSkill_charging");

                            canonDirectionAngle = PlayerControl.facingRight ? 0f : 180f;

                            if (waterCanonObj!=null) { Destroy(waterCanonObj); waterCanonObj = null; }
                            waterCanonObj = Instantiate(waterCanonPrefab, this.transform.position,Quaternion.identity, this.transform);
                            waterCanonObj.transform.localScale = Vector3.one;

                            specialSkill_state = SpecialSkill_State.charging;
                        }
                        break;

                    //charging中
                    case SpecialSkill_State.charging:

                        if (!isSpitting)
                        {
                            WaterCanon_AngleInput();
                            SetOKA_direction(canonDirectionAngle);
                        }
                        
                        break;
                }
            }
        }
        //釋放
        else if (Input.GetButtonUp("Attack"))
        {
            specialInput_holdTime = 0f;

            if (specialSkill_state == SpecialSkill_State.charging)
            {
                animator.SetTrigger("specialSkill_release");
                waterCanonObj.transform.GetChild(0).GetComponent<Animator>().SetTrigger("release");
                waterCanonObj.transform.GetChild(0).GetComponent<special_l_waterCanon>().SetAngle(canonDirectionAngle);

                playerEnergy.ModifyWaterEnergy(-specialCost);

                SetSpitting(true);

                //反作用力
                if (reactionF_routine != null) StopCoroutine(reactionF_routine);
                reactionF_routine = StartCoroutine(ReactionForce());

                specialSkill_state = SpecialSkill_State.throwout;
            }
        }
        //噴發中
        else if (specialSkill_state == SpecialSkill_State.throwout)
        {
            
        }
    }

    #region ================↓毛細滲透相關↓================

    public void WaitPassInput(Collider2D passTrigger) //由playerControl的TriggerStay呼叫
    {
        if (Input.GetButtonDown("Special") && !isPassing && PlayerStatus.canSkill)
        {
            whatWaterPassing = passTrigger.transform.parent.gameObject;

            if (passTrigger.transform.parent.name == "tree_passing")
            {
                TreePassBegin();
                PlayerStatus.isWaterPassing = true;
            }
            else if (passTrigger.transform.parent.name == "hole_passing")
            {
                HolePassBegin();
                PlayerStatus.isWaterPassing = true;
            }
        }
    }

    //從treePass or holePass 設定waterdropTrans
    public void SetWaterdropTrans(Transform Trans_waterdrop) { waterdropTrans = Trans_waterdrop; }

    void TreePassBegin()
    {
        isPassing = true;
        playerControl.noticeUIControl.NoticeUI_Setting(999);

        //主角下跳anim 並由anim event觸發TreePass
        if (whatWaterPassing.name == "tree_passing") animator.SetTrigger("treePassBegin");
        else Debug.Log("anim trigger ERR");
    }
    void TreePass() //由anim event觸發
    {
        SetOkaRenderAndCol(false);

        whatWaterPassing.GetComponent<TreePass>().WaterPassing(this.transform.gameObject);
        waterdropPassing = true;
    }
    public void TreePassOver(Vector3 exitPoint, bool facingRight) //由treePass 呼叫
    {
        waterdropPassing = false;

        //讓主角出現再設定好的出口位置
        StartCoroutine(PlayerTransLerp(exitPoint, 0.3f));

        animator.SetTrigger("passOut");
        StartCoroutine(OpenRendererDelay());
    }

    void HolePassBegin()
    {
        isPassing = true;
        playerControl.noticeUIControl.NoticeUI_Setting(999);

        //anim event觸發HolePass
        if (whatWaterPassing.name == "hole_passing") animator.SetTrigger("horiHoleBegin");
        else Debug.Log("anim trigger ERR");
    }
    void HolePass() //由anim event觸發
    {
        SetOkaRenderAndCol(false);

        whatWaterPassing.GetComponent<HolePass>().WaterPassing(this.transform.gameObject);
        waterdropPassing = true;
    }
    public void HolePassOver(Vector3 exitPoint, float exitAngle) //由holePass 呼叫
    {
        waterdropPassing = false;

        //讓主角出現再設定好的出口位置
        StartCoroutine(PlayerTransLerp(exitPoint, 0.1f));

        animator.SetTrigger("passOut");
        StartCoroutine(OpenRendererDelay()); //若同時animator.SetTrigger("passOut") & 開啟renderer 會有BUG
    }

    IEnumerator OpenRendererDelay() { yield return new WaitForSeconds(0.01f); SetOkaRenderAndCol(true); }
    //由PassOut animation呼叫
    public void SetMoveableAfterPassOut() { PlayerStatus.isWaterPassing = false; isPassing = false; }
    public void PassingInterrupted() { isPassing = false; PlayerStatus.isWaterPassing = false; }
    void SetOkaRenderAndCol(bool truefalse)
    {
        spriteRenderer.enabled = truefalse;
        playerCollider.enabled = truefalse;
    }
    IEnumerator PlayerTransLerp(Vector3 goalPos,float inTime)
    {
        Vector3 fromPos = playerTrans.position;
        float elapsed = 0f;

        while (elapsed < inTime)
        {
            elapsed += Time.deltaTime;
            playerTrans.position = Vector3.Lerp(fromPos, goalPos, elapsed / inTime);
            yield return null;
        }
    }

    #endregion ================↑毛細滲透相關↑================

    void Attack3_FX_Init()
    {
        GameObject FX = Instantiate(Attack3_FX, this.transform.position, Quaternion.identity);
        if (!PlayerControl.facingRight) FX.transform.localScale = new Vector3(-FX.transform.localScale.x, FX.transform.localScale.y, FX.transform.localScale.z);
    }

    #region special skill
    void WaterCanon_AngleInput()
    {
        if (PlayerStatus.Get_isKeyboard())
        {
            float xInput = Input.GetAxis("Horizontal");
            float yInput = Input.GetAxis("Vertical");

            if (Mathf.Abs(xInput) >= 0.1f || Mathf.Abs(yInput) >= 0.1f)
            {
                canonDirectionAngle = Mathf.Atan2(yInput, xInput);
                canonDirectionAngle = canonDirectionAngle / Mathf.PI * 180f;
            }
        }
        else
        {
            float xInput = Input.GetAxis("XBOX_Horizontal");
            float yInput = Input.GetAxis("XBOX_Vertical");

            if (Mathf.Abs(xInput) >= 0.1f || Mathf.Abs(yInput) >= 0.1f)
            {
                canonDirectionAngle = Mathf.Atan2(-yInput, xInput);
                canonDirectionAngle = canonDirectionAngle / Mathf.PI * 180f;
            }
        }
    }
    void SetOKA_direction(float toDegree)
    {
        if (toDegree > 90f) toDegree = 180f - toDegree;
        else if (toDegree < -90f) toDegree = -180 - toDegree;
       
        this.transform.localEulerAngles = new Vector3(0f, 0f, toDegree);
    }
    void SpecialAttacking(bool truefalse)
    {
        attacking = truefalse;
        PlayerStatus.isSpecialSkilling = truefalse;
    }
    void SetSpitting(bool truefalse)
    {
        isSpitting = truefalse;
        PlayerStatus.canControl = !truefalse;
    }
    //反作用力(力量會由大變小)
    IEnumerator ReactionForce()
    {
        float delayTime = 0.1f;
        yield return new WaitForSeconds(delayTime);

        float reactionF_angle = canonDirectionAngle / 180f * Mathf.PI;
        Vector2 reactionF_direction = new Vector2(-Mathf.Cos(reactionF_angle), -Mathf.Sin(reactionF_angle));
        float Force = 100f;

        while (true)
        {
            playerControl.rb2d.AddForce(reactionF_direction * Force);
            if (Force > 10f) Force *= 0.1f; //讓後退速度變慢但又不至於完全停止

            yield return null;
        }
    }
    void canonSpitStop()
    {
        if (waterCanonObj != null)
        {
            waterCanonObj.transform.SetParent(null);
            waterCanonObj = null;
        }
        SetSpitting(false);
        SetOKA_direction(0f);

        //停止反作用力
        if (reactionF_routine != null) StopCoroutine(reactionF_routine);

        specialSkill_state = SpecialSkill_State.none;
    }
    void SpecialSkillOver()
    {
        SpecialAttacking(false);
    }
    #endregion
}
