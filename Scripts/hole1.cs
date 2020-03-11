using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hole1 : MonoBehaviour {

    public int scene_level = 1;
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().GoToScene(scene_level);
        }
    }

    //public void ChangeScene()
    //{
    //    SceneManager.LoadScene(1);
    //}
}
