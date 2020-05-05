using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MagicStele : MonoBehaviour {

    private SpriteRenderer stele_glow;
    private CanvasGroup canvasGroup;

    private float fadeTime = 0.3f;

    // Use this for initialization
    void Awake ()
    {
        stele_glow = this.transform.Find("stele_glow").GetComponent<SpriteRenderer>();
        stele_glow.color = new Color(1f, 1f, 1f, 0f);

        canvasGroup = this.transform.Find("Canvas").GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") { canvasGroup.DOFade(1f, fadeTime); }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") { canvasGroup.DOFade(0f, fadeTime); }
    }
}
