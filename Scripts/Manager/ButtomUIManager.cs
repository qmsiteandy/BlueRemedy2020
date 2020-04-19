using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtomUIManager : MonoBehaviour {

    [Header("按鍵UI切換")]  //keyboard按鍵和joystick按鍵
    public GameObject keyboardButtonGroup;
    public GameObject joystickButtonGroup;
    private bool isKeyboardInput = true;


    void Start()
    {
        if (keyboardButtonGroup == null) Debug.LogError("keyboardButtonGroup is null");
        if (joystickButtonGroup == null) Debug.LogError("joystickButtonGroup is null");

        SetButtonUIState(PlayerStatus.Get_isKeyboard());

    }

    void Update ()
    {
        if (isKeyboardInput && !PlayerStatus.Get_isKeyboard()) { isKeyboardInput = false; SetButtonUIState(isKeyboardInput); }
        else if (!isKeyboardInput && PlayerStatus.Get_isKeyboard()) { isKeyboardInput = true; SetButtonUIState(isKeyboardInput); }
    }

    void SetButtonUIState(bool isKeyboardInput)
    {
        if (isKeyboardInput) { keyboardButtonGroup.SetActive(true); joystickButtonGroup.SetActive(false); isKeyboardInput = true; }
        else { keyboardButtonGroup.SetActive(false); joystickButtonGroup.SetActive(true); isKeyboardInput = false; }
    }
}
