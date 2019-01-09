using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStepOn : MonoBehaviour {

    public float stepJumpForce = 375f;  //踩踏敵人後的彈跳力道

    public CameraShake cameraShake;//用來呼叫攝影機震動函式，之後刪

    private PlayerControl playerControl;  //儲存母物件的PlayerMove腳本
    private Rigidbody2D rb2d;   //儲存母物件的Rigidbody2D元件

    void Start () {

        //讀取母物件的PlayerMove腳本
        playerControl = GetComponentInParent<PlayerControl>();
        //讀取母物件的Rigidbody2D元件
        rb2d = GetComponentInParent<Rigidbody2D>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "GrassZone")
        {
            other.GetComponent<GrassDiv>().GrassGrow();
        }
        //判斷碰撞物是否為敵人頭部區域
        else if (other.gameObject.name == "EnemyHead")
            //判斷主角是否騰空且下降中才可踩踏
            if (!playerControl.grounded && rb2d.velocity.y<=0)
            {
                //儲存此敵人的EnemyTest腳本
                Enemy_base enemy_base = other.gameObject.GetComponentInParent<Enemy_base>();

                //判斷此敵人是否醒著
                if (enemy_base.isAwake)
                {
                    //先將主角垂直速度歸零
                    rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
                    //給主角踩踏跳躍力
                    rb2d.AddForce(Vector2.up * stepJumpForce);
                }

                //呼叫此敵人的IsSteppedOn函式
                enemy_base.IsSteppedOn();
            }

    }

}
