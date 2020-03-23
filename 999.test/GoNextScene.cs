using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoNextScene : MonoBehaviour {

    public int scene_level = 2;
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
        GameObject.Find("GameManager").GetComponent<GameManager>().GoToScene(scene_level);

    }
}
