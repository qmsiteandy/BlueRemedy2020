using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStele : MonoBehaviour {

    private SpriteRenderer stele_glow;
    private CanvasGroup canvasGroup;

    private float fadeUpSpeed = 5f;
    private float fadeDownSpeed = 4f;
    private float alpha = 0f;
    private bool isFadingUp = false;


    // Use this for initialization
    void Awake ()
    {
        stele_glow = this.transform.Find("stele_glow").GetComponent<SpriteRenderer>();
        stele_glow.color = new Color(1f, 1f, 1f, 0f);

        canvasGroup = this.transform.Find("Canvas").GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (isFadingUp && alpha < 1f)
        {
            alpha += fadeUpSpeed * Time.deltaTime;
            if (alpha > 1f) alpha = 1f;
        }
        else if (!isFadingUp && alpha > 0f)
        {
            alpha -= fadeDownSpeed * Time.deltaTime;
            if (alpha < 0f) alpha = 0f;
        }

        canvasGroup.alpha = alpha;
        stele_glow.color = new Color(1f, 1f, 1f, alpha);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") { isFadingUp = true; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") { isFadingUp = false; }
    }
}
