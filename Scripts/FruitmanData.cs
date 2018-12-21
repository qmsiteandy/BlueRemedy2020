using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//暫時沒用到的腳本
public class FruitmanData : MonoBehaviour{

    public static List<FruitmanInfo> fruitmanList = new List<FruitmanInfo>();

    public struct FruitmanInfo
    {
        public string name;
        public int[] cmd;
        public int fressment;

        public int attack;
        public int defence;
        public int heal;

        public int season;
        public int occupy;

        public FruitmanInfo(string nameIn, int cmd1, int cmd2, int cmd3, int cmd4, 
            int fressIn, int atkIn, int defIn, int healIn, int seasonIn, int occupyIn)
        {
            this.name = nameIn;
            this.cmd = new int[4] { cmd1, cmd2, cmd3, cmd4 };
            this.fressment = fressIn;
            this.attack = atkIn;
            this.defence = defIn;
            this.heal = healIn;
            this.season = seasonIn;
            this.occupy = occupyIn;
        }
    }

    void Start()
    {
        fruitmanList.Add(new FruitmanInfo("蘋果人", 1, 2, 3, 4, 1000, 200, 150, 500, 2, 2));
        fruitmanList.Add(new FruitmanInfo("西瓜人", 2, 3, 4, 1, 1000, 200, 150, 500, 2, 2));
    }
    
}
