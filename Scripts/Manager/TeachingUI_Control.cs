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

    [Header("按鍵UI切換")]  //keyboard按鍵和joystick按鍵
    public bool hasButtonUI = false;
    public GameObject keyboardButtonGroup;
    public GameObject joystickButtonGroup;
    private bool isKeyboardInput = true;

    // Use this for initialization
    void Start ()
    {
        alpha = 0f;

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = alpha;

        if (hasButtonUI) SetButtonUIState(isKeyboardInput);
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

        if (hasButtonUI)
        {
            if (isKeyboardInput && !PlayerStatus.Get_isKeyboard()) { isKeyboardInput = false; SetButtonUIState(isKeyboardInput); }
            else if(!isKeyboardInput && PlayerStatus.Get_isKeyboard()) { isKeyboardInput = true; SetButtonUIState(isKeyboardInput); }
        } 
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

    void SetButtonUIState(bool isKeyboardInput)
    {
        if (isKeyboardInput) { keyboardButtonGroup.SetActive(true); joystickButtonGroup.SetActive(false); }
        else { keyboardButtonGroup.SetActive(false); joystickButtonGroup.SetActive(true); }
    }
}
