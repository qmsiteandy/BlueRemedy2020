using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Water : Skill_Base
{
    private float holePassingSped = 0.07f;
    [HideInInspector] public bool isPassing = false;
    private bool waterdropPassing = false;
    private GameObject whatWaterPassing;
    private Transform waterdropTrans;
    GameObject playerPointLight;

    // Use this for initialization
    void Start ()
    {
        BaseStart();
    }

    // Update is called once per frame
    void Update ()
    {
        BaseUpdate();

        if (waterdropPassing)
        {
            playerTrans.position = waterdropTrans.position;
        }  
    }

    #region ================↓毛細滲透相關↓================

    public void PassBegin(Collider2D passTrigger) //由playerControl的TriggerStay呼叫
    {
        if (Input.GetButtonDown("Special") && !isPassing)
        {
            whatWaterPassing = passTrigger.transform.parent.gameObject;

            if (passTrigger.transform.parent.name == "tree_passing")
            {
                TreePassBegin();
            }
            else if (passTrigger.transform.parent.name == "hole_passing")
            {
                HolePassBegin();
            }
        }
    }

    //從treePass or holePass 設定waterdropTrans
    public void SetWaterdropTrans(Transform Trans_waterdrop) { waterdropTrans = Trans_waterdrop; }

    void TreePassBegin()
    {
        isPassing = true;
        playerControl.noticeUI.SetActive(false);
        PlayerControl.canMove = false;

        //主角下跳anim 並由anim event觸發TreePass
        if (whatWaterPassing.name == "tree_passing") animator.SetTrigger("treePassBegin");
        else Debug.Log("anim trigger ERR");
    }
    void TreePass() //由anim event觸發
    {
        SetOkaRenderAndCol(false);

        whatWaterPassing.GetComponent<TreePass>().WaterPassing(this.gameObject);
        cameraControl.SetTarget(waterdropTrans, 0.01f);
        waterdropPassing = true;
    }
    public void TreePassOver(Vector3 exitPoint, bool facingRight) //由treePass 呼叫
    {
        waterdropPassing = false;

        //讓主角出現再設定好的出口位置
        StartCoroutine(PlayerTransLerp(exitPoint, 0.3f));

        animator.SetTrigger("passOut");
        StartCoroutine(OpenRendererDelay());

        cameraControl.SetTarget(this.gameObject.transform, 0.01f);
    }

    void HolePassBegin()
    {
        isPassing = true;
        playerControl.noticeUI.SetActive(false);
        PlayerControl.canMove = false;

        //anim event觸發HolePass
        if (whatWaterPassing.name == "hole_passing") animator.SetTrigger("horiHoleBegin");
        else Debug.Log("anim trigger ERR");
    }
    void HolePass() //由anim event觸發
    {
        SetOkaRenderAndCol(false);

        whatWaterPassing.GetComponent<HolePass>().WaterPassing(this.gameObject);
        cameraControl.SetTarget(waterdropTrans, 0.01f);
        waterdropPassing = true;
    }
    public void HolePassOver(Vector3 exitPoint, float exitAngle) //由holePass 呼叫
    {
        waterdropPassing = false;

        //讓主角出現再設定好的出口位置
        StartCoroutine(PlayerTransLerp(exitPoint, 0.1f));

        animator.SetTrigger("passOut");
        StartCoroutine(OpenRendererDelay()); //若同時animator.SetTrigger("passOut") & 開啟renderer 會有BUG

        cameraControl.SetTarget(this.gameObject.transform, 0.01f);
    }

    IEnumerator OpenRendererDelay() { yield return new WaitForSeconds(0.001f); SetOkaRenderAndCol(true); }
    //由PassOut animation呼叫
    public void SetMoveableAfterPassOut() { PlayerControl.canMove = true; isPassing = false; }
    public void PassingInterrupted() { isPassing = false; PlayerControl.canMove = true; }
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
}
