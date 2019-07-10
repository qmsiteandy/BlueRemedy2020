using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterArea : MonoBehaviour {

    [Header("水對主角影響")]
    public float waterDrag = 2.0f;
    public float speedDownRate = 0.8f;

    //主要影響冰的飄浮
    [Header("波浪")] 
    public bool makeWave = false;
    public float waveCrest;//波浪最高點;
    public float waveFreq = 1f; //波浪頻率
    public float waveHeight = 0.3f; //波浪高度;
    private float waveMid;
    public float waveUpdateCycle = 0.5f; //波峰高度更新週期
    public bool waveCrestDebugOpen = false;


    void Start()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (!makeWave) waveCrest = collider.transform.position.y + collider.offset.y + collider.size.y / 2;
        else waveMid = collider.transform.position.y + collider.offset.y + collider.size.y / 2;

        if (makeWave) StartCoroutine(MakeWave());

        if (waveCrestDebugOpen)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.GetComponent<Transform>().position = new Vector3(transform.position.x, waveCrest, transform.position.z);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {

        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {

        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {

        }
    }

    IEnumerator MakeWave()
    {
        //運用sin函式搭配設定的頻率、波高來製造波浪高度
        float timer = 0f;
        float anglePerUpdate, angleNow = 0f;
        float oneRoundAngle = Mathf.PI * 2;

        anglePerUpdate = waveFreq * 360 * waveUpdateCycle;
        anglePerUpdate = anglePerUpdate * Mathf.PI / 180;

        while(true)
        {
            timer += Time.deltaTime;
            if (timer>= waveUpdateCycle)
            {
                angleNow += anglePerUpdate;
                if (angleNow >= oneRoundAngle) angleNow -= oneRoundAngle;
                Debug.Log("angleNow" + angleNow+ " oneRoundAngle "+ oneRoundAngle);

                waveCrest = waveMid + waveHeight * Mathf.Sin(angleNow);
                if (waveCrestDebugOpen) transform.GetChild(0).GetComponent<Transform>().position = new Vector3(transform.position.x, waveCrest, transform.position.z);

                timer = 0f;
            }
            yield return 0;
        } 
    }
}
