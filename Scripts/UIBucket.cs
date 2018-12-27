using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBucket : MonoBehaviour {

    public Text occupyNow;
    public Text occupyMax;

    // Use this for initialization
    void Start () {
        occupyNow.text = "0";
        occupyMax.text = "/ " + FruitManager.occupyLimit;
    }
	
    public void SetOccupyNum(int change)
    {
        occupyNow.text = "" + change;
    }
}
