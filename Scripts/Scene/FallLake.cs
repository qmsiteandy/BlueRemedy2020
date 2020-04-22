using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallLake : MonoBehaviour {

    public GameObject Ice;
    private CanAtkObj canatkobj;
    private Animator animator;

	// Use this for initialization
	void Start () {
        canatkobj = Ice.GetComponentInChildren<CanAtkObj>();
        animator = transform.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(canatkobj.isDead == true)
        {
            animator.SetTrigger("Fall");
        }
	}
}
