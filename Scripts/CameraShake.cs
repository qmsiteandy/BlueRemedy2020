using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    //Shake函式，引數為震動長度及震動幅度
	public IEnumerator Shake(float duration,float magnitude)
    {
        //儲存原始camera位置
        Vector3 originalPos = transform.localPosition;

        //已經過的時間(計時器)
        float elapsed = 0f;

        while (elapsed < duration)
        {
            //震動位置
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            //設定camera位置
            transform.localPosition = new Vector3(x, y, originalPos.z);

            //計時
            elapsed += Time.deltaTime;

            //每偵跳脫
            yield return null;
        }
        //回到camera起始位置
        transform.localPosition = originalPos;
    }
}
