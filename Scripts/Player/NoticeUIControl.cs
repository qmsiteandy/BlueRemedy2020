using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeUIControl : MonoBehaviour {

    private GameObject[] noticeUI;
    private Animator noticeBG_animator;
    private int noticeIndexNow = 0;
    private bool isOpen = false;

    void Awake ()
    {
        noticeBG_animator = this.transform.Find("noticeBG_thinking").GetComponent<Animator>();

        noticeUI = new GameObject[this.transform.Find("mark_folder").childCount];
        for (int x = 0; x < noticeUI.Length; x++) noticeUI[x] = this.transform.Find("mark_folder").GetChild(x).gameObject;

        //一開始先關閉renderer才不會看到disappear動畫，renderer在open動畫中會再被開啟
        this.transform.Find("noticeBG_thinking").GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        this.transform.localScale = new Vector3(PlayerControl.facingRight ? 1f : -1f, 1f, 1f);
    }

    //NoticeUI setting
    public void NoticeUI_Setting(int index)
    {
        if (noticeUI.Length == 0) return;
        if (index != noticeIndexNow)
        {
            //開啟整個noticeUI_folder
            if (index < noticeUI.Length && isOpen == false)
            {
                noticeBG_animator.SetTrigger("open");
                StartCoroutine(OpenMarkDelay(0.2f));
                isOpen = true;
            }
            //關閉整個noticeUI_folder
            else if (index >= noticeUI.Length && isOpen == true)
            {
                noticeBG_animator.SetTrigger("close");
                CloseMark();
                isOpen = false;
            }

            //顯示or關閉UI項目
            for (int x = 0; x < noticeUI.Length; x++) { noticeUI[x].SetActive(false); }
            if (index < noticeUI.Length) { noticeUI[index].SetActive(true); }

            noticeIndexNow = index;
        }
    }

    IEnumerator OpenMarkDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        this.transform.Find("mark_folder").gameObject.SetActive(true);
    }
    void CloseMark()
    {
        this.transform.Find("mark_folder").gameObject.SetActive(false);
    }
}
