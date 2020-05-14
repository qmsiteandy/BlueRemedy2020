using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
	private static int PlayerInWhichLevel;
    private static int LevelRecord = 0;

    public static void set_PlayerInWhichLevel(int level) { PlayerInWhichLevel = level; }
    public static void ClearLevel(int level) { if (level > LevelRecord) LevelRecord = level; }

    public static int get_PlayerInWhichLevel() { return (PlayerInWhichLevel); }
    public static int get_LevelRecord() { return (LevelRecord); }
}

public class VedioData
{
    private static List<string> storyRead = new List<string>();

    public static void ComleteStoryVedio(string newStoryName)
    {
        storyRead.Add(newStoryName);
    }

    public static bool hasTheStoryRead(string storyName)
    {
        foreach(string name in storyRead)
        {
            if (name == storyName) { return (true); }
        }
        return (false); 
    }
}


