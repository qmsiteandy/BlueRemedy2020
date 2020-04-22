using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogMove : MonoBehaviour {

    public float floatSpeed = -0.2f;
    public float leftLimit = -30f;
    public float rightLimit = 100f;
    public bool isLeft = true;

    void Update()
    {
        if (isLeft)
        {
            transform.position += new Vector3(floatSpeed * Time.deltaTime, 0f, 0f);

            if (this.transform.position.x < leftLimit) transform.position = new Vector3(rightLimit, this.transform.position.y, this.transform.position.z);
        }
        else
        {
            transform.position += new Vector3(floatSpeed * Time.deltaTime, 0f, 0f);

            if (this.transform.position.x > rightLimit) transform.position = new Vector3(leftLimit, this.transform.position.y, this.transform.position.z);
        }
        
    }
}
