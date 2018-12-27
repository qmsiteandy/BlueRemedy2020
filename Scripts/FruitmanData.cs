public class FruitmanData {

    public static Info[] InfoList =
    {
        //ID=0
        new Info("Player",new int[]{ 0, 0, 0, 0 },false,0,0,0,0,0,0),

        //ID=1
        new Info(
            "AppleMan",                 //name
            new int[]{ 1, 1, 1, 1 },    //cmd
            true,                       //can_call 
            200,                        //fressment
            200,                        //attack
            200,                        //defence
            600,                        //heal
            3,                          //season
            1),                         //cooupy

        //ID=2
        new Info(
            "WatermelonMan",            //name
            new int[]{ 1, 2, 1, 2 },    //cmd
            true,                       //can_call 
            400,                        //fressment
            200,                        //attack
            500,                        //defence
            200,                        //heal
            2,                          //season
            2),                         //cooupy
    };

    public class Info
    {
        public string name;
        public int[] cmd = { 0, 0, 0, 0 };
        public bool can_call;
        public int fressment;

        public int attack;
        public int defence;
        public int heal;

        public int season;
        public int occupy;

        public Info(string nameIn, int[] cmdIn, bool can_callIn, int fressIn, int atkIn, int defIn, int healIn, int seasonIn, int occupyIn)
        {
            name = nameIn;
            cmd = cmdIn;
            can_call = can_callIn;
            fressment = fressIn;
            attack = atkIn;
            defence = defIn;
            heal = healIn;
            season = seasonIn;
            occupy = occupyIn;
        }
    }
}
