using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBrunch : MonoBehaviour {

    GameObject whole_branch_sprite;
    GameObject branch_1;
    GameObject branch_2;
    GameObject branch_3;
    GameObject branch_4;

    // Use this for initialization
    void Start () {
        branch_1 = transform.GetChild(0).gameObject;
        branch_2 = transform.GetChild(1).gameObject;
        branch_3 = transform.GetChild(2).gameObject;
        branch_4 = transform.GetChild(3).gameObject;
        whole_branch_sprite = transform.GetChild(4).gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            whole_branch_sprite.SetActive(false);
            branch_1.SetActive(true);
            branch_2.SetActive(true);
            branch_3.SetActive(true);
            branch_4.SetActive(true);
        }
    }
}
