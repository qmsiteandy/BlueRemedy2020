using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Area : MonoBehaviour {

    //主要影響冰的飄浮
    [Header("波浪")] 
    public bool makeWave = false;   //是否有波浪
    [HideInInspector] public float waveCrest; //波浪最高點，設公開，主角漂浮時會讀取
    private float waveCrestOffsetY = 0f;
    public float waveFreq = 1.6f; //設定波浪頻率
    public float waveHeight = 0.1f; //設定波浪高度;
    private float waveUpdateFreq = 8f; //波峰高度更新頻率

    [Header("對主角增加髒汙")]
    public bool isDirtyWater = false;
    public int addDirtAmount = 5;
    public float damageCycle = 2f;   //持續在水中，弄髒的週期
    private Coroutine Damage_Routine;
    private PlayerControl playerControl;

    void Start()
    {
        //取得WaterArea的collider並設定波浪平均高度
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        waveCrestOffsetY = collider.offset.y + collider.size.y / 2 + 0.2f;
        waveCrest = transform.position.y + waveCrestOffsetY;

        //造浪
        if (makeWave) StartCoroutine(MakeWave());
    }

    void Update()
    {
        waveCrest = transform.position.y + waveCrestOffsetY;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerControl = collider.GetComponent<PlayerControl>();

            if (isDirtyWater)
            {
                if(Damage_Routine != null) { StopCoroutine(Damage_Routine); }
                Damage_Routine = StartCoroutine(DamageCycle());
            }

        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerControl = null;

            if (isDirtyWater)
            {
                if (Damage_Routine != null) { StopCoroutine(Damage_Routine); Damage_Routine = null; }
            }
        }
    }

    IEnumerator DamageCycle()
    {
        float damageTimer = 0f;

        while (true)
        {
            if (damageTimer <= 0f)
            {
                playerControl.TakeDamage(0, addDirtAmount);
                damageTimer = damageCycle;
            }
            else
            {
                damageTimer -= Time.deltaTime;
            }

            yield return null;
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
            if (timer >= 1/waveUpdateFreq)
            {
                anglePerUpdate = waveFreq * 360 / waveUpdateFreq;
                angleNow += anglePerUpdate;
                if (angleNow >= 360f) angleNow -= 360f;

                angleConvert = angleNow * Mathf.PI / 180;

                waveCrest += waveHeight * Mathf.Sin(angleConvert);

                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
                yield return null;
            } 
        }
    }
}
