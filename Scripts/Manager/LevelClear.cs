using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelClear : MonoBehaviour {

    private bool isChange = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(isChange == false)
            {
                ChangeScene();
            }
        }
    }

    public void ChangeScene()
    {
        isChange = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().LevelClear(SceneManager.GetActiveScene().buildIndex);

    }
}
