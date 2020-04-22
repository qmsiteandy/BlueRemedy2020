using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtomUIManager : MonoBehaviour {

    [Header("按鍵UI切換")]  //keyboard按鍵和joystick按鍵
    public GameObject keyboardButtonGroup;
    public GameObject joystickButtonGroup;

    static private List<GameObject> keyboardUI_List = new List<GameObject>();
    static private List<GameObject> joystickUI_List = new List<GameObject>();


    static private bool isKeyboardInput = true;


    void Awake()
    {
        if (keyboardButtonGroup == null) Debug.LogError("keyboardButtonGroup is null");
        if (joystickButtonGroup == null) Debug.LogError("joystickButtonGroup is null");

        keyboardUI_List.Add(keyboardButtonGroup);
        joystickUI_List.Add(joystickButtonGroup);

        SetButtonUIState(PlayerStatus.Get_isKeyboard());

    }
    void OnDestroy()
    {
        keyboardUI_List.Remove(keyboardButtonGroup);
        joystickUI_List.Remove(joystickButtonGroup);
    }

    void Update ()
    {
        if (isKeyboardInput && !PlayerStatus.Get_isKeyboard()) { isKeyboardInput = false; SetButtonUIState(isKeyboardInput); }
        else if (!isKeyboardInput && PlayerStatus.Get_isKeyboard()) { isKeyboardInput = true; SetButtonUIState(isKeyboardInput); }
    }

    static void SetButtonUIState(bool isKeyboardInput)
    {
        if (isKeyboardInput)
        {
            for (int x = 0; x < keyboardUI_List.Count; x++) keyboardUI_List[x].SetActive(true);
            for (int x = 0; x < joystickUI_List.Count; x++) joystickUI_List[x].SetActive(false);

            isKeyboardInput = true;
        }
        else
        {
            for (int x = 0; x < keyboardUI_List.Count; x++) keyboardUI_List[x].SetActive(false);
            for (int x = 0; x < joystickUI_List.Count; x++) joystickUI_List[x].SetActive(true);

            isKeyboardInput = false;
        }
    }
}
