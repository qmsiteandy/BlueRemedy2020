using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourManager : MonoBehaviour {

    public PlayerControl playerControl;
    public FruitmanCall fruitmanCall;
    public SkillModeControl skillModeControl;

    public void BehaviourPause(bool isPause)
    {
        playerControl.allCanDo = !isPause;
        fruitmanCall.canCall = !isPause;
        skillModeControl.SkillPause(isPause);
    }

}
