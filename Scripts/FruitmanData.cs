using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitmanData {

    List<FruitmanInfo> fruitmanList = new List<FruitmanInfo>();

    struct FruitmanInfo
    {
        string name;
        int[] cmd;
        int fressment;

        int attack;
        int defence;
        int heal;

        int season;
        int occupy;
    }
    
}
