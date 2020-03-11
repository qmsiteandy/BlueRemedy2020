using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class panel : MonoBehaviour {

    private Animation animation;
	// Use this for initialization
	void Start () {
        animation = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void FadeIn()
    {
        animation.Play("FadeIn");
    }

    public void FadeOut()
    {
        animation.Play("FadeOut");
    }
}
