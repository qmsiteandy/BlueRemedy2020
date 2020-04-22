using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broken_trunk_Control : MonoBehaviour {

    private Animator animator;
    public GameObject broken_trunk_1;
    public GameObject broken_trunk_2;
    private EdgeCollider2D edgecollider2d;
    private PlayerStatus PlayerStatus;

	// Use this for initialization
	void Start () {
        animator = transform.GetComponent<Animator>();
        edgecollider2d = transform.GetComponentInChildren<EdgeCollider2D>();
        PlayerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            animator.SetTrigger("Broken");
        }
    }

    public void trunkOpen()
    {
        edgecollider2d.enabled = false;
        broken_trunk_1.SetActive(true);
        broken_trunk_2.SetActive(true);
    }

    public void PlayerStop()
    {
        PlayerStatus.canControl = false;
    }

    public void PlayerContinue()
    {
        PlayerStatus.canControl = true;
    }
}
