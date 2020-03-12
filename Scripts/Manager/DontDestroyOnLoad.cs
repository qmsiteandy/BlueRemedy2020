using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad : MonoBehaviour
{
    static List<GameObject> dontDestroyList = new List<GameObject>();

    void Awake()
    {
        if(CompareTheSame(this.name) == false)
        {
            DontDestroyOnLoad(this);
            dontDestroyList.Add(this.gameObject);
            //Debug.Log("添加" + SceneManager.GetActiveScene().name + "的" + this.name);
        }
        else
        {
            Destroy(this.gameObject);
            //Debug.Log("刪除" + SceneManager.GetActiveScene().name + "的" + this.name);
        }
    }

    bool CompareTheSame(string name)
    {
        bool hasTheSame = false;

        foreach(GameObject obj in dontDestroyList)
        {
            if(name == obj.name)
            {
                hasTheSame = true;
                return hasTheSame;
            }
        }
        return hasTheSame;
    }
}
