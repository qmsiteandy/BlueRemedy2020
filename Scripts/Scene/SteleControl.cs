using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteleControl : MonoBehaviour {

    private SpriteRenderer stele_glow;

    private float fadeUpSpeed = 5f;
    private float fadeDownSpeed = 4f;
    private float alpha = 0f;
    private bool isFadingUp = false;

    // Use this for initialization
    void Awake(){
        stele_glow = this.transform.Find("stele_glow").GetComponent<SpriteRenderer>();
        stele_glow.color = new Color(1f, 1f, 1f, 0f);
    }
	
	// Update is called once per frame
	void Update () {
        if (isFadingUp && alpha < 1f)
        {
            alpha += fadeUpSpeed * Time.deltaTime;
            if (alpha > 1f) alpha = 1f;
        }

        stele_glow.color = new Color(1f, 1f, 1f, alpha);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") { isFadingUp = true; }
    }

}
