using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Area : MonoBehaviour {

    [Header("水對主角影響")]
    public float waterDrag = 2.0f;  //水對主角阻力
    public float speedDownRate = 0.8f;  //水導致主角降速比例

    //主要影響冰的飄浮
    [Header("波浪")] 
    public bool makeWave = false;   //是否有波浪
    [HideInInspector] public float waveCrest; //波浪最高點，設公開，主角漂浮時會讀取
    public float waveFreq = 1.6f; //設定波浪頻率
    public float waveHeight = 0.1f; //設定波浪高度;
    private float waveMid;  //波浪平均高度
    private float waveUpdateFreq = 8f; //波峰高度更新頻率

    [Header("對主角增加髒汙")]
    public bool isDirtyWater = false;
    public int addDirtAmount = 5;
    public float damageCycle = 2f;   //持續在水中，弄髒的週期
    private bool isPlayerInWater = false;
    private PlayerControl playerControl;
    private Coroutine routineWaterDamage;

    //public WaterLine waterLine;

    void Start()
    {
        //取得WaterArea的collider並設定波浪平均高度
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (!makeWave) waveCrest = collider.transform.position.y + collider.offset.y + collider.size.y / 2 + 0.2f;
        else waveMid = collider.transform.position.y + collider.offset.y + collider.size.y / 2 + 0.2f;

        //造浪
        if (makeWave) StartCoroutine(MakeWave());

        //waterLine = GetComponent<WaterLine>();
    }

    void Update()
    {
        if (isPlayerInWater && isDirtyWater)
        {
            if (PlayerControl.OkaID_Now == 0) return;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            //跳入水面製造波浪
            float playerYSpeed = collider.GetComponent<PlayerControl>().rb2d.velocity.y;
            //waterLine.Splash(collider.transform.position, Mathf.Lerp(-0.5f, -3.5f, playerYSpeed / -20f));

            playerControl = collider.GetComponent<PlayerControl>();
            PlayerControl.speedLimit *= speedDownRate;
            playerControl.isInWater = true;

            Rigidbody2D rb2d = collider.GetComponent<Rigidbody2D>();
            rb2d.drag = waterDrag;

            isPlayerInWater = true;

            //髒水傷害主角
            if (isDirtyWater)
            {
                if (routineWaterDamage != null) { StopCoroutine(routineWaterDamage); routineWaterDamage = null; }
                routineWaterDamage = StartCoroutine(DamageRoutine(addDirtAmount, damageCycle));
            }
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerControl.InWater();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            //跳出水面製造波浪
            float playerYSpeed = collider.GetComponent<PlayerControl>().rb2d.velocity.y;
            //waterLine.Splash(collider.transform.position, Mathf.Lerp(0.5f, 3.5f, playerYSpeed / -20f));

            playerControl = collider.GetComponent<PlayerControl>();
            PlayerControl.speedLimit = playerControl.initSpeedLimit;
            playerControl.isInWater = false;

            Rigidbody2D rb2d = collider.GetComponent<Rigidbody2D>();
            rb2d.drag = 0f;

            isPlayerInWater = false;
            playerControl = null;

            if (routineWaterDamage != null) { StopCoroutine(routineWaterDamage); routineWaterDamage = null; }
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

    IEnumerator DamageRoutine(int amount, float delay)
    {
        float timer = 0f;

        while(true)
        {
            timer -= Time.deltaTime;

            if ( timer <=0f && isPlayerInWater != null)
            {
                playerControl.TakeDamage(0, amount);
                timer = delay; 
            }
            yield return null;
        } 
    }
}
