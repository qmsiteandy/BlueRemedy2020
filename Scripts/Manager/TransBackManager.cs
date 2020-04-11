using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransBackManager : MonoBehaviour {

    //可以設定玩家進入設定好的collider後傳回上一個recordPoint
    //或也可以直接互叫函式傳回

    private Transform playerTrans;
    //private CameraControl cameraControl;

    [Header("Dark Panel")]
    private Image darkBlockPanel;
    private float fadeSpeed = 0.08f;

	void Start ()
    {
        darkBlockPanel = GameObject.Find("GameManager").transform.GetChild(0).GetChild(0).GetComponent<Image>();
        darkBlockPanel.gameObject.SetActive(true);
        darkBlockPanel.color = new Color(0f, 0f, 0f, 0f);

        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        //cameraControl = GameObject.Find("CameraHolder").GetComponent<CameraControl>();
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
        
        while (darkBlockPanel.color.a < 1f) { darkBlockPanel.color += new Color(0f, 0f, 0f, fadeSpeed); yield return null; }
        PlayerStatus.isTransingBack = true;

        //cameraControl.SetCameraPos(RecordPointManager.Get_playerRecordPos());
        playerTrans.position = RecordPointManager.Get_playerRecordPos();

        yield return new WaitForSeconds(0.5f);

        PlayerStatus.isTransingBack = false;
        while (darkBlockPanel.color.a > 0f) { darkBlockPanel.color -= new Color(0f, 0f, 0f, fadeSpeed); yield return null; }
    }
}
