using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

    public float rotateAmount = 20f; // Amount to be rotated

    private Animator animator;
    private bool isGrowed = false;
    private Light flowerLight;

    private Quaternion oriAngle;

    private bool isParticlePlayed = false;
    public GameObject FollowGrowParticle;
    public Transform particlePoint;

    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();

        oriAngle = transform.rotation;

        flowerLight = transform.GetChild(1).GetComponent<Light>();
        flowerLight.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!isGrowed) GrowFlower();

            if(PlayerControl.facingRight)
                StartCoroutine(RotateMe(Vector3.forward * -rotateAmount, 0.1f));
            else
                StartCoroutine(RotateMe(Vector3.forward * rotateAmount, 0.1f));
        }     
    }

    void GrowFlower()
    {
        isGrowed = true;
        animator.SetTrigger("grow");
    }

    IEnumerator RotateMe(Vector3 byAngles, float inTime)
    {
        Quaternion fromAngle = transform.rotation;
        Quaternion toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);

        for (float t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation =  Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }
        for (float t = 1f; t>=0; t -= Time.deltaTime / inTime * 3f)
        {
            transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }
        transform.rotation = oriAngle;

        if (!isParticlePlayed) FollowParticle();
    }

    //animation呼叫
    void OpenLight()
    {
        flowerLight.enabled = true;
    }

    void FollowParticle()
    {
        isParticlePlayed = true;

        GameObject particle = Instantiate(FollowGrowParticle, particlePoint.position, Quaternion.identity);
        Destroy(particle, 5f);
    }



}
