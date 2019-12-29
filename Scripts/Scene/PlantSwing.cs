using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSwing : MonoBehaviour {

    [Header("擺動")]
    public float maxSwinOffset = 0.5f;
    public float maxSwinSpeed = 0.14f;
    public float swinbackSpeed = 0.14f;
    private float swinToAngle, swinSpeed;
    private float xOffset = 0f;
    private Material material;

    void Start ()
    {
        material = GetComponent<SpriteRenderer>().material;
        this.GetComponent<BoxCollider2D>().size *= new Vector2(0.7f, 1f);   //collider窄一點搖擺更順暢，否則會花很多時間等player exit 
    }

    void Update()
    {
        //---擺動---
        if (xOffset != swinToAngle)
        {
            xOffset = Mathf.Lerp(xOffset, swinToAngle, swinSpeed);
            if (Mathf.Abs(xOffset - swinToAngle) < 0.1f) xOffset = swinToAngle;
            material.SetFloat("_xOffset", xOffset);
        }
        else { swinToAngle = 0f; swinSpeed = swinbackSpeed; }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //---擺動---
            if (Mathf.Abs(PlayerControl.xSpeed) > 4f)
            {
                float speedRate = PlayerControl.xSpeed / PlayerControl.speedLimit;
                swinToAngle = speedRate * maxSwinOffset;
                swinSpeed = Mathf.Abs(speedRate * maxSwinSpeed);
            }
            else { swinToAngle = 0f; swinSpeed = swinbackSpeed; }
        }
    }
}
