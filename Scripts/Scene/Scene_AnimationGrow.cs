using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_AnimationGrow : MonoBehaviour {

    private bool isGrowed = false;
    private bool isShaked = false;
    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isGrowed == false) Grow();
        }
    }

    void Grow()
    {
        isGrowed = true;
        animator.SetTrigger("Grow");
    }

    public void Shake()
    {
        if(isShaked == false)
        {
            isShaked = true;
            animator.SetTrigger("Shake");
        }
    }
}
