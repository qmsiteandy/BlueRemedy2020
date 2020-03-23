using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushrInteract : MonoBehaviour {

    private EdgeCollider2D platformCol;
    private EdgeCollider2D platformTrig;
    private Vector3[] platformColPoints;
    private Transform[] BoneTransList;

    private Transform Left_IK_Point, Right_IK_Point;
    private Vector3 OriPlatformMid;
    private Vector3 OriLeftPos, OriRightPos;
    //private Vector3 LeftIK_AimPos, RightIK_AimPos;

    public bool OkaStepOn = false;
    private PlayerControl playerControl;

    private MushEndInteract leftEnd_mushInteract, rightEnd_mushInteract;

    void Start()
    {
        platformCol = this.GetComponent<EdgeCollider2D>();
        platformCol.edgeRadius *= this.transform.localScale.x;
        Init_PlatformColPoints();
        Init_BoneTrans();

        platformTrig = this.gameObject.AddComponent<EdgeCollider2D>();
        platformTrig.points = platformCol.points;
        platformTrig.isTrigger = true;
        platformTrig.edgeRadius = platformCol.edgeRadius * 2f;

        Left_IK_Point = transform.Find("Left_CCDSolver2D");
        Right_IK_Point = transform.Find("Right_CCDSolver2D");

        OriLeftPos = Left_IK_Point.position;
        OriRightPos = Right_IK_Point.position;

        leftEnd_mushInteract = Left_IK_Point.GetComponent<MushEndInteract>();
        rightEnd_mushInteract = Right_IK_Point.GetComponent<MushEndInteract>();

    }

    void Update()
    {
        //更新EdgeCollider points
        Vector2[] colliderpoints = platformCol.points;
        for (int x = 0; x < platformCol.pointCount; x++)
        {
            colliderpoints[x] = BoneTransList[x].position - platformCol.transform.position;
            colliderpoints[x].x /= transform.lossyScale.x;
            colliderpoints[x].y /= transform.lossyScale.y;
        }
        platformTrig.points = platformCol.points = colliderpoints;
    }

    #region 初始化
    void Init_PlatformColPoints()
    {
        platformColPoints = new Vector3[platformCol.pointCount];
        for (int x = 0; x < platformCol.pointCount; x++)
        {
            platformColPoints[x] = platformCol.transform.position +
                new Vector3(platformCol.points[x].x * transform.lossyScale.x, platformCol.points[x].y * transform.lossyScale.y, 0f);
            
        }

    }

    void Init_BoneTrans()
    {
        BoneTransList = new Transform[platformCol.pointCount];
        BoneTrans_CpmpareAndLink(this.transform.Find("root"), platformColPoints);

    }

    void BoneTrans_CpmpareAndLink(Transform rootTrans, Vector3[] colVecs)
    {
        if (rootTrans.childCount >= 1) BoneTrans_CpmpareAndLink(rootTrans.GetChild(0), colVecs);
        if (rootTrans.childCount >= 2)
        {
            BoneTrans_CpmpareAndLink(rootTrans.GetChild(1), colVecs);
            OriPlatformMid = rootTrans.GetChild(1).position;
        }

        for (int Index = 0; Index < colVecs.Length; Index++)
        {
            if (BoneTransList[Index] == null)
                if (ComparePoint(rootTrans.position, new Vector3(colVecs[Index].x, colVecs[Index].y)))
                {
                    BoneTransList[Index] = rootTrans;
                }
        }
    }

    bool ComparePoint(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.4f;
    }
    #endregion
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !OkaStepOn)
        {
            playerControl = collision.transform.GetComponent<PlayerControl>();

            OkaStepOn = true;
        } 
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && OkaStepOn)
        {
            float leftPosRatio = 0f, rightPosRatio = 0f;
            if (collision.transform.position.x < OriPlatformMid.x)
            {
                leftPosRatio = (collision.transform.position.x - OriPlatformMid.x) / (OriLeftPos.x - OriPlatformMid.x);
                rightPosRatio = -(leftPosRatio / (OriPlatformMid.x - OriLeftPos.x) * (OriRightPos.x - OriPlatformMid.x)) * 0.2f;
            }
            else
            {
                rightPosRatio = (collision.transform.position.x - OriPlatformMid.x) / (OriRightPos.x - OriPlatformMid.x);
                leftPosRatio = -(rightPosRatio / (OriRightPos.x - OriPlatformMid.x) * (OriPlatformMid.x - OriLeftPos.x)) * 0.2f;
            }

            leftEnd_mushInteract.AddYForce(leftPosRatio * 15f);
            rightEnd_mushInteract.AddYForce(rightPosRatio * 15f);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") { OkaStepOn = false; }
    }
}
