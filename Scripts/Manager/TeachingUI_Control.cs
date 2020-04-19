using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TeachingUI_Control : MonoBehaviour {

    [Header("UI顯現")]
    public float fadeUpSpeed = 5f;
    public float fadeDownSpeed = 4f;
    private CanvasGroup canvasGroup;
    private float alpha;
    private bool isFadingUp = false;

    // Use this for initialization
    void Start ()
    {
        alpha = 0f;

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = alpha;
    }

    // Update is called once per frame
    void Update ()
    {
        if (isFadingUp && canvasGroup.alpha < 1f)
        {
            alpha += fadeUpSpeed * Time.deltaTime;
            if (alpha > 1f) alpha = 1f;
        }
        else if (!isFadingUp && canvasGroup.alpha > 0f)
        {
            alpha -= fadeDownSpeed * Time.deltaTime;
            if (alpha < 0f) alpha = 0f;
        }

        canvasGroup.alpha = alpha;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isFadingUp = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isFadingUp = false;
        }
    }
}
