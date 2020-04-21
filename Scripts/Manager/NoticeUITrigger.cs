using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeUITrigger : MonoBehaviour {

    public int callNoticeUI_index = 99;
    private PlayerControl playerControl;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerControl = collision.GetComponent<PlayerControl>();

            if(callNoticeUI_index != PlayerControl.OkaID_Now) playerControl.noticeUIControl.NoticeUI_Setting(callNoticeUI_index);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerControl.noticeUIControl.NoticeUI_Setting(99);
        }
    }
}
