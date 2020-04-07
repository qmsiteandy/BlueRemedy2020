using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogMove : MonoBehaviour {

    float floatSpeed = -2f;
    public float leftLimit = 91f;
    public float rightLimit = -13f;

    void Update()
    {
        transform.position += new Vector3(floatSpeed * Time.deltaTime, 0f, 0f);

        if (this.transform.position.x < leftLimit) transform.position = new Vector3(rightLimit, this.transform.position.y, this.transform.position.z);
    }
}
