using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWheel : MonoBehaviour {

    [Header("轉輪物件")]
    private CanvasGroup WheelCanvas;
    private bool wheelShow = false;
    private int lightIndex = 1;

    private GameObject[] selected_light = { null, null, null };
    private GameObject[] input_arrows_UI = { null, null, null };
    private GameObject[] input_buttons_UI = { null, null, null };
    
    private PlayerChange playerChange;

    //[Header("按鍵UI切換")]  //keyboard按鍵和joystick按鍵
    //public GameObject keyboardButtonGroup;
    //public GameObject joystickButtonGroup;
    //private bool isKeyboardInput = true;


    void Start()
    {
        playerChange = transform.parent.GetComponent<PlayerChange>();

        WheelCanvas = GetComponent<CanvasGroup>();
        WheelCanvas.alpha = 0f;

        for (int i = 0; i < 3; i++) { selected_light[i] = transform.GetChild(0).transform.GetChild(i).gameObject; selected_light[i].SetActive(false); }
        /*for (int i = 0; i < 3; i++)
        {
            input_arrows_UI[i] = transform.GetChild(2).GetChild(0).GetChild(i).gameObject; input_arrows_UI[i].SetActive(false);
            input_buttons_UI[i] = transform.GetChild(2).GetChild(1).GetChild(i).gameObject; input_buttons_UI[i].SetActive(false);
        }*/
        
        //SetButtonUIState(isKeyboardInput);
    }

    void Update()
    {
        if (wheelShow)
        {
            for (int i = 0; i < 3; i++)
            {
                //input_arrows_UI[i].SetActive(true);
                //input_buttons_UI[i].SetActive(true);
                selected_light[i].SetActive(false);
            }
            switch (lightIndex)
            {
                case (0):
                    {
                        //input_arrows_UI[0].SetActive(false);
                        //input_buttons_UI[0].SetActive(false);
                        selected_light[0].SetActive(true);
                    }
                    break;
                case (1):
                    {
                        //input_arrows_UI[1].SetActive(false);
                        //input_buttons_UI[1].SetActive(false);
                        selected_light[1].SetActive(true);
                    }
                    break;
                case (2):
                    {
                        //input_arrows_UI[2].SetActive(false);
                        //input_buttons_UI[2].SetActive(false);
                        selected_light[2].SetActive(true);
                    }
                    break;
                default: break;
            }
            
            /*if (isKeyboardInput && !PlayerStatus.Get_isKeyboard()) { isKeyboardInput = false; SetButtonUIState(isKeyboardInput); }
            else if (!isKeyboardInput && PlayerStatus.Get_isKeyboard()) { isKeyboardInput = true; SetButtonUIState(isKeyboardInput); }*/
        }
    }

    public void WheelShow()
    {
        WheelCanvas.alpha = 1f;
        lightIndex = PlayerControl.OkaID_Now;
        wheelShow = true;
    }

    public void WheelDisappear()
    {
        WheelCanvas.alpha = 0f;
        wheelShow = false;
    }

    public void WheelIndexSelect(int index)
    {
        lightIndex = index;
    }


    public void LightFlash(int index)
    {
        StartCoroutine(Flash(index));
    }
    IEnumerator Flash(int index)
    {
        float flashDelay = 0.1f;

        if (wheelShow) selected_light[index].SetActive(false);
        yield return new WaitForSeconds(flashDelay);
        if (wheelShow) selected_light[index].SetActive(true);
        yield return new WaitForSeconds(flashDelay);
        if (wheelShow) selected_light[index].SetActive(false);
        yield return new WaitForSeconds(flashDelay);
        if (wheelShow) selected_light[index].SetActive(true);
    }

    /*void SetButtonUIState(bool isKeyboardInput)
    {
        if (isKeyboardInput) { keyboardButtonGroup.SetActive(true); joystickButtonGroup.SetActive(false); }
        else { keyboardButtonGroup.SetActive(false); joystickButtonGroup.SetActive(true); }
    }*/

    public bool Get_wheelShow() { return (wheelShow); }
}
