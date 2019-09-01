using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyWater : MonoBehaviour {

    [Header("水對主角影響")]
    public float waterDrag = 2.0f;  //水對主角阻力
    public float speedDownRate = 0.8f;  //水導致主角降速比例

    //主要影響冰的飄浮
    [Header("波浪")] 
    public bool makeWave = false;   //是否有波浪
    public float waveCrest; //波浪最高點，設公開，主角漂浮時會讀取
    public float waveFreq = 1.6f; //設定波浪頻率
    public float waveHeight = 0.1f; //設定波浪高度;
    private float waveMid;  //波浪平均高度
    public float waveUpdateFreq = 8f; //波峰高度更新頻率

    [Header("對主角增加髒汙")]
    public int addDirtAmount = 5;
    public float damageCycle = 2f;   //持續在水中，弄髒的週期
    private float damageTimer = 0f;
    private bool isPlayerInWater = false;
    private PlayerControl playerControl;



    void Start()
    {
        //取得WaterArea的collider並設定波浪平均高度
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (!makeWave) waveCrest = collider.transform.position.y + collider.offset.y + collider.size.y / 2 + 0.2f;
        else waveMid = collider.transform.position.y + collider.offset.y + collider.size.y / 2 + 0.2f;

        //造浪
        if (makeWave) StartCoroutine(MakeWave());
    }

    void Update()
    {
        if (isPlayerInWater)
        {
            if (playerControl.Oka_ID == 0) return;

            if (damageTimer >= damageCycle)
            {
                playerControl.TakeDamage(0, addDirtAmount);
                damageTimer = 0f;
            }
            else
            {
                damageTimer += Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log(collider.gameObject.name + " in");
            playerControl = collider.GetComponent<PlayerControl>();
            playerControl.speedLimit *= speedDownRate;
            playerControl.isInWater = true;

            Rigidbody2D rb2d = collider.GetComponent<Rigidbody2D>();
            rb2d.drag = waterDrag;

            isPlayerInWater = true;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerControl = collider.GetComponent<PlayerControl>();

            playerControl.InWater();
        }

    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log(collider.gameObject.name + " out");
            playerControl = collider.GetComponent<PlayerControl>();
            playerControl.speedLimit = playerControl.initSpeedLimit;
            playerControl.isInWater = false;

            Rigidbody2D rb2d = collider.GetComponent<Rigidbody2D>();
            rb2d.drag = 0f;

            isPlayerInWater = false;
            playerControl = null;
            damageTimer = 0f;
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
                
                timer = 0f;
            }
            yield return 0;
        }
    }
}
