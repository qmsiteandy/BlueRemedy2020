using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransBackManager : MonoBehaviour {

    //可以設定玩家進入設定好的collider後傳回上一個recordPoint
    //或也可以直接互叫函式傳回

    private Transform playerTrans;
    //private CameraControl cameraControl;
    private GameManager gameManager;



	void Start ()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        //cameraControl = GameObject.Find("CameraHolder").GetComponent<CameraControl>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Transback");
            Vector3 backPos = RecordPointManager.Get_playerRecordPos();
            StartCoroutine(TransBack(backPos));
        }
    }

    public void DieTransBack()
    {
        Vector3 backPos = RecordPointManager.Get_playerRecordPos();
        StartCoroutine(TransBack(backPos));
    }

    IEnumerator TransBack(Vector3 TransBackPos)
    {

        gameManager.BlackFadeInOut(true, 0.3f);
        PlayerStatus.isTransingBack = true;

        //cameraControl.SetCameraPos(RecordPointManager.Get_playerRecordPos());
        playerTrans.position = RecordPointManager.Get_playerRecordPos();

        yield return new WaitForSeconds(0.5f);

        PlayerStatus.isTransingBack = false;
        gameManager.BlackFadeInOut(false, 0.3f);
    }
}
