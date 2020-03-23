using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hole1 : MonoBehaviour {

    public int scene_level = 1;
    private bool isChange = false;
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(isChange == false)
            {
                if (Input.GetKey(KeyCode.Return))
                {
                    ChangeScene();
                }
            }
        }
    }

    public void ChangeScene()
    {
        //SceneManager.LoadScene(1);
        isChange = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().GoToScene(scene_level);
    }
}
