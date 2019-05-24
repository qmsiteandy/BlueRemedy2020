using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWheel : MonoBehaviour {

    [Header("轉輪物件")]
    private SpriteRenderer wheelLight;
    private Transform wheelBack;
    private Transform[] iconsTrans = { null, null, null };
    public bool isSpinFinish = true;
    private int spinIndexTarget = 1;
    private float angleNow, angleTarget;
    private float wheelLightAlpha = 0f;

    private bool turningStop = true;
    private PlayerChange playerChange;


    void Start ()
    {
        playerChange = transform.parent.GetComponent<PlayerChange>();

        wheelLight =this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        wheelBack = this.transform.GetChild(1).transform;
        wheelLight.color = new Color(1f, 1f, 1f, 0f);
        wheelBack.gameObject.SetActive(false);

        for (int i = 0; i < 3; i++) iconsTrans[i] = transform.GetChild(1).transform.GetChild(i).GetComponent<Transform>();

        
    }
	
	void Update ()
    {
        this.transform.position = playerChange.Oka_form[playerChange.form_now].transform.position;
    }

    public void WheelShow()
    {
        wheelLight.color = new Color(1f, 1f, 1f, 1f);
        wheelBack.gameObject.SetActive(true);
    }

    public void WheelDisappear()
    {
        turningStop = true;
        wheelLight.color = new Color(1f, 1f, 1f, 0f);
        wheelBack.gameObject.SetActive(false);
    }

    public void WheelSpinRight(bool isTurnRight)
    {
        turningStop = false;
        StartCoroutine(Spin(isTurnRight));
    }

    IEnumerator Spin(bool isTurnRight)
    {
        isSpinFinish = false;

        while (wheelLight.color.a > 0 && !turningStop)
        {
            wheelLightAlpha = wheelLight.color.a;
            wheelLightAlpha -= 0.25f;
            wheelLight.color = new Color(1f, 1f, 1f, wheelLightAlpha);

            yield return null;
        }

        angleNow = wheelBack.eulerAngles.z;
        
        if (isTurnRight) angleTarget = angleNow - 120f; else angleTarget = angleNow + 120f;

        while (angleNow != angleTarget)
        {
            angleNow = Mathf.Lerp(angleNow, angleTarget, 0.3f);
            if (Mathf.Abs(angleNow - angleTarget) < 5f) angleNow = angleTarget;
            

            wheelBack.rotation = Quaternion.Euler(0f, 0f, angleNow);

            for (int i = 0; i < 3; i++) iconsTrans[i].rotation = Quaternion.Euler(0f, 0f, 0f);

            yield return null;
        }

        while (wheelLight.color.a < 1 && !turningStop)
        {
            wheelLightAlpha = wheelLight.color.a;
            wheelLightAlpha += 0.25f;
            wheelLight.color = new Color(1f, 1f, 1f, wheelLightAlpha);

            yield return null;
        }

        if(turningStop) wheelLight.color = new Color(1f, 1f, 1f, 0f);

        isSpinFinish = true;
    }


}
