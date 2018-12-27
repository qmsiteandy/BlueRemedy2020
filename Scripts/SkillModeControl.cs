using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillModeControl : MonoBehaviour {

    public GameObject[] skillObject;

    private int modeNow = 0;

    void Start()
    {
        skillObject = new GameObject[transform.childCount + 1];

        skillObject[0] = null;
        for (int x = 0; x < transform.childCount; x++)
        {
            skillObject[x + 1] = transform.GetChild(x).gameObject;
        }
    }
    
    public void SetSkillMode(int id)
    {
        if (modeNow != 0)
        {
            skillObject[modeNow].SetActive(false);
        }

        modeNow = id;

        if (modeNow != 0)
        {
            skillObject[modeNow].SetActive(true);
        }
    }
}
