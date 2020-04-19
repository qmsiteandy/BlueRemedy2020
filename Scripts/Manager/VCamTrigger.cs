using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCamTrigger : MonoBehaviour {

    private void Start()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") this.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
