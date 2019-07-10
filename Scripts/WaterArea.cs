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
    public float waveFreq = 1.6f; //波浪頻率
    public float waveHeight = 0.1f; //波浪高度;
    private float waveMid;
    public float waveUpdateFreq = 8f; //波峰高度更新頻率
    [Space(10)]
    public bool waveCrestDebugOpen = false;


    void Start()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (!makeWave) waveCrest = collider.transform.position.y + collider.offset.y + collider.size.y / 2 + 0.2f;
        else waveMid = collider.transform.position.y + collider.offset.y + collider.size.y / 2 + 0.2f;

        if (makeWave) StartCoroutine(MakeWave());

        if (waveCrestDebugOpen)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.GetComponent<Transform>().position = new Vector3(transform.position.x, waveCrest, transform.position.z);
        }
        else transform.GetChild(0).gameObject.SetActive(false);
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
        float anglePerUpdate;
        float angleNow = 0f, angleConvert;
      
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >= 1/waveUpdateFreq)
            {
                anglePerUpdate = waveFreq * 360 / waveUpdateFreq;
                angleNow += anglePerUpdate;
                if (angleNow >= 360f) angleNow -= 360f;

                angleConvert = angleNow * Mathf.PI / 180;
                waveCrest = waveMid + waveHeight * Mathf.Sin(angleConvert);
                if (waveCrestDebugOpen) transform.GetChild(0).GetComponent<Transform>().position = new Vector3(transform.position.x, waveCrest, transform.position.z);

                timer = 0f;
            }
            yield return 0;
        }
    }
}
