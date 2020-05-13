using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Manager : MonoBehaviour {

    public AudioClip[] BGM;

    private PlayerStatus.Season musicSeason;
    private AudioSource audioSource;
    private Coroutine opencloseRoutine;

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource.clip == null) audioSource.clip = BGM[0];

        audioSource.Play();
        audioSource.volume = 0f;
        OpenBGMInTime(1f);
    }

    void Update ()
    {
        if(PlayerStatus.get_inSeason() != musicSeason)
        {
            switch (PlayerStatus.get_inSeason())
            {
                case PlayerStatus.Season.none:
                    
                    break;

                case PlayerStatus.Season.spring:
                    SetBGM_Clip(BGM[0], 0.5f);
                    break;

                case PlayerStatus.Season.summer:

                    break;

                case PlayerStatus.Season.fall:
                    SetBGM_Clip(BGM[1], 0.5f);
                    break;

                case PlayerStatus.Season.winter:
                    SetBGM_Clip(BGM[2], 0.5f);
                    break;
            }
            musicSeason = PlayerStatus.get_inSeason();
        }
    }

    void SetBGM_Clip(AudioClip BGM_clip, float fadeTime)
    {
        StartCoroutine(I_SetBGM_Clip(BGM_clip, fadeTime));  
    }
    IEnumerator I_SetBGM_Clip(AudioClip BGM_clip, float fadeTime)
    {
        CloseBGMInTime(fadeTime);


        yield return new WaitForSeconds(fadeTime);
        audioSource.clip = BGM_clip;

        OpenBGMInTime(fadeTime);
    }

    public void OpenBGMInTime(float inTime)
    {
        if (opencloseRoutine != null) { StopCoroutine(opencloseRoutine); opencloseRoutine = null; }
        opencloseRoutine = StartCoroutine(OpenCloseBGM(true, inTime));
    }
    public void CloseBGMInTime(float inTime)
    {
        if (opencloseRoutine != null) { StopCoroutine(opencloseRoutine); opencloseRoutine = null; }
         opencloseRoutine = StartCoroutine(OpenCloseBGM(false, inTime));
    }
    IEnumerator OpenCloseBGM(bool open, float inTime)
    {
        if (open)
        {
            audioSource.Play();
            while (audioSource.volume < 1f)
            {
                
                audioSource.volume += Time.deltaTime / inTime;
                yield return null;
            }
        }
        else
        {
            while (audioSource.volume > 0f)
            {
                audioSource.volume -= Time.deltaTime / inTime;
                yield return null;
            }
            audioSource.Stop();
        }
        
    }
}
