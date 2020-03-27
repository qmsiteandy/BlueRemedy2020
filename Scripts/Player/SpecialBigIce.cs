using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBigIce : MonoBehaviour {

    private Water_Area water_area;
    private int WaterLayerID = 14;
    private Rigidbody2D rb2d;
    public float boxColiderSize;
    private float FloatForce = 35f;
    private bool isInWater = false;
    private PlayerControl playerControl;
    public float player_xOffset = 0f;
    


    void Awake ()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace)) Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == WaterLayerID)
        {
            water_area = collision.GetComponent<Water_Area>();
            rb2d.drag = 3f;

            isInWater = true;
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == WaterLayerID)
        {
            if (water_area.waveCrest - transform.position.y + (0.5f * boxColiderSize) > 0.8f * boxColiderSize) rb2d.AddForce(Vector2.up * FloatForce);
            else if (water_area.waveCrest - transform.position.y + (0.5f * boxColiderSize) > 0f) rb2d.AddForce(Vector2.up * FloatForce * (water_area.waveCrest - transform.position.y / 1f));

            int angleID = (int)(Mathf.Floor(transform.eulerAngles.z % 360f / 45f));

            if (angleID == 0) RotateIceAngle(0f); 
            else if (angleID == 1 || angleID == 2) RotateIceAngle(90f);
            else if (angleID == 3 || angleID == 4) RotateIceAngle(180f);
            else if (angleID == 5 || angleID == 6) RotateIceAngle(270f);
            else if(angleID == 7) RotateIceAngle(360f);
        }   
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        isInWater = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerControl = collision.gameObject.GetComponent<PlayerControl>();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (playerControl.objUnderFoot == this.gameObject)
            {
                if(boxColiderSize > 1f)
                {
                    if (playerControl.xInput == 0)
                    {
                        collision.transform.position = new Vector3((Mathf.Lerp(collision.transform.position.x + player_xOffset, this.transform.position.x, 0.3f)), collision.transform.position.y, collision.transform.position.z);
                        rb2d.AddForce(Vector3.up * FloatForce * 1.3f);
                    }
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerControl = null;
        }
    }

    void RotateIceAngle(float toAngle)
    {
        float rotationZ = Mathf.Lerp(transform.eulerAngles.z, toAngle, 0.8f); if (Mathf.Abs(rotationZ - toAngle) < 0.02f) rotationZ = toAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }

    public void ThrowOut(float destroyTime)
    {
        boxColiderSize = GetComponent<BoxCollider2D>().size.y * transform.localScale.y;

        StartCoroutine(DestroyAfterTime(destroyTime));
    }

    IEnumerator DestroyAfterTime(float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }
}
