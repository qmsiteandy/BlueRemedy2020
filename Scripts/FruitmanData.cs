﻿public class FruitmanData {

    public static Info[] InfoList =
    {
        //ID=0
        new Info("Player",new int[]{ 0, 0, 0, 0 },false,0,0,0,0,0,0,0,0),

        //ID=1
        new Info(
            "Apple",                    //name
            new int[]{ 1, 1, 1, 1 },    //cmd
            true,   //can_call 
            1000,   //fresh
            200,    //attack
            200,    //defence
            600,    //heal
            200,    //attack_freshLoss
            100,    //defence_freshLoss
            3,      //season
            1),     //cooupy

        //ID=2
        new Info(
            "Watermelon",               //name
            new int[]{ 1, 2, 1, 2 },    //cmd
            true,   //can_call 
            1500,   //fresh
            200,    //attack
            500,    //defence
            200,    //heal
            200,    //attack_freshLoss
            400,    //defence_freshLoss
            2,      //season
            2),     //cooupy
    };

    public class Info
    {
        public string name;
        public int[] cmd = { 0, 0, 0, 0 };
        public bool can_call;
        public int fresh;

        public int attack;
        public int defence;
        public int heal;

        public int attack_freshLoss;
        public int defence_freshLoss;

        public int season;
        public int occupy;

        public Info(string nameIn, int[] cmdIn, bool can_callIn, int freshIn, int atkIn, int defIn, int healIn, int a_freshLoss, int d_freshLoss, int seasonIn, int occupyIn)
        {
            name = nameIn;
            cmd = cmdIn;
            can_call = can_callIn;
            fresh = freshIn;
            attack = atkIn;
            defence = defIn;
            heal = healIn;
            attack_freshLoss = a_freshLoss;
            defence_freshLoss = d_freshLoss;
            season = seasonIn;
            occupy = occupyIn;
        }
    }
}
