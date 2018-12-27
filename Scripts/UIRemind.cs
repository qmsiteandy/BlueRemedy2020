using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRemind : MonoBehaviour {

    public Image remindMark;
    public Text remindText;
    public float shortRemindDelay = 0.5f;

    private string remindNow="";


    public void LongRemind(string content)
    {
        remindMark.enabled = true;
        remindText.enabled = true;

        remindText.text = content;
        remindNow = content;
    }

    public void ShortRemind(string content)
    {
        remindText.text = content;
        remindMark.enabled = true;
        remindText.enabled = true;

        StartCoroutine(Delay_Return());     
    }

    public void DeleteRemind()
    {
        remindMark.enabled = false;
        remindText.enabled = false;

        remindText.text = "";
        remindNow = "";
    }

    IEnumerator Delay_Return()
    {
        yield return new WaitForSeconds(shortRemindDelay);

        if (remindNow == "")
        {
            remindMark.enabled = false;
            remindText.enabled = false;
        }
        else
        {
            remindText.text = remindNow;
        }
    }
}
