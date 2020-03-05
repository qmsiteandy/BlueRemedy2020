using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour{
    
    //===Status===
    public static bool isSkilling = false;
    public static bool isChanging = false;
    public static bool isWallSticking = false;
    public static bool isTransingBack = false;
    public static bool isInInteractTrigger = false;
    public static bool isWaterPassing = false;

    //===CanDoWhat===
    public static bool canMove = true;
    public static bool canJump = true;
    public static bool canSkill = true;
    public static bool canChange = true;
    public static bool canBeHurt = true;

    //===InputMode===
    private static bool isJoystickConnected = false;
    private static bool keyboardInput = true;   //若否則為搖桿

    private void Start()
    {
        isJoystickConnected = Input.GetJoystickNames().Length > 0;
    }

    private void Update()
    {
        canMove = canJump = true;
        canSkill = true;
        canChange = true;
        canBeHurt = true;

        if (isSkilling)
        {
            canMove = canJump = false;
            canChange = false;
        }
        else if (isChanging)
        {
            canMove = canJump = false;
            canSkill = false;
            canBeHurt = false;
            canChange = false;
        }
        else if (isWallSticking)
        {
            canSkill = false;
            canChange = false;
        }
        else if (isTransingBack)
        {
            canMove = canJump = false;
            canSkill = false;
            canChange = false;
            canBeHurt = false;
        }
        else if (isInInteractTrigger)
        {
            canJump = false;
        }
        else if (isWaterPassing)
        {
            canMove = canJump = false;
            canSkill = false;
            canChange = false;
            canBeHurt = false;
        }
    }

#region inputMode

    //OnGUI自動Update，不須放入Update函式
    void OnGUI()
    {
        if (!isJoystickConnected) return;
        switch (keyboardInput)
        {
            case true:
                if (isJoystick()) keyboardInput = false;
                break;
            case false:
                if (isKeyboard()) keyboardInput = true;
                break;
        }
    }
    private bool isKeyboard()
    {
        // mouse & keyboard buttons
        if (Event.current.isKey ||
            Event.current.isMouse)
        {
            return true;
        }
        //// mouse movement
        //if (Input.GetAxis("Mouse X") != 0.0f ||
        //    Input.GetAxis("Mouse Y") != 0.0f)
        //{
        //    return true;
        //}
        return false;
    }
    private bool isJoystick()
    {
        // joystick buttons
        if (Input.GetKey(KeyCode.JoystickButton0) ||
           Input.GetKey(KeyCode.JoystickButton1) ||
           Input.GetKey(KeyCode.JoystickButton2) ||
           Input.GetKey(KeyCode.JoystickButton3) ||
           Input.GetKey(KeyCode.JoystickButton4) ||
           Input.GetKey(KeyCode.JoystickButton5) ||
           Input.GetKey(KeyCode.JoystickButton6) ||
           Input.GetKey(KeyCode.JoystickButton7) ||
           Input.GetKey(KeyCode.JoystickButton8) ||
           Input.GetKey(KeyCode.JoystickButton9) ||
           Input.GetKey(KeyCode.JoystickButton10))
        {
            return true;
        }
        // joystick axis
        if (Input.GetAxis("XBOX_Horizontal") != 0.0f ||
           Input.GetAxis("XBOX_Vertical") != 0.0f ||
           Input.GetAxis("XBOX_Change") != 0.0f)
        {
            return true;
        }
        return false;
    }
    static public bool Get_isKeyboard()
    {
        return keyboardInput;
    }

#endregion
}
