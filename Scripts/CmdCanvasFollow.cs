using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CmdCanvasFollow : MonoBehaviour {

    private Transform player;
    private Vector3 offset;
    private RectTransform rectTransform;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        rectTransform = GetComponent<RectTransform>();

        offset = rectTransform.anchoredPosition3D - player.position;
        Debug.Log(offset);
    }
	
	// Update is called once per frame
	void Update () {
        rectTransform.anchoredPosition3D = player.position + offset;
    }
}
