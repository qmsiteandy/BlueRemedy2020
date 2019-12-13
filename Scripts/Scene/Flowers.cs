using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flowers : MonoBehaviour {

    [Header("花")]
    public bool isMajorFolower = false;
    private bool isGrowed = false;
    private Light flowerLight;
    private Animator animator;

    [Header("長小花")]
    public float firstGrowDelay = 0.7f;
    public float delayBetween = 0.2f;
    public Transform minorFlowers_folder;
    public Flowers[] minorFlowers;

    [Header("特效")]
    public GameObject FollowGrowParticle;
    private bool isParticlePlayed = false;
    public Transform particlePoint;

    [Header("擺動")]
    public float rotateAmount = 20f; // Amount to be rotated
    private Quaternion oriAngle;


    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();

        oriAngle = transform.rotation;


        if (isMajorFolower)
        {
            flowerLight = transform.GetChild(1).GetComponent<Light>();
            flowerLight.enabled = false;

            minorFlowers = new Flowers[minorFlowers_folder.childCount];
            for (int x = 0; x < minorFlowers_folder.childCount; x++) minorFlowers[x] = minorFlowers_folder.GetChild(x).GetComponent<Flowers>();
        } 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!isGrowed && isMajorFolower) MajorFlowerGrow();

            if (PlayerControl.facingRight)
                StartCoroutine(RotateMe(Vector3.forward * -rotateAmount, 0.1f));
            else
                StartCoroutine(RotateMe(Vector3.forward * rotateAmount, 0.1f));
        }
    }

    //主花生長
    void MajorFlowerGrow()
    {
        isGrowed = true;
        animator.SetTrigger("grow");
        StartCoroutine(MinorFlowerGrow(firstGrowDelay, delayBetween));
    }

    //小花生長
    public void MinorFlowerGrow()
    {
        if (!isGrowed)
        {
            isGrowed = true;
            animator.SetTrigger("grow");
        } 
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

        if (isMajorFolower && !isParticlePlayed) FollowParticle();
    }

    //animation呼叫
    void OpenLight()
    {
        if (isMajorFolower) flowerLight.enabled = true;
    }

    void FollowParticle()
    {
        isParticlePlayed = true;

        GameObject particle = Instantiate(FollowGrowParticle, particlePoint.position, Quaternion.identity);
        Destroy(particle, 5f);
    }

    IEnumerator MinorFlowerGrow(float firstGrowDelay, float delayBetween)
    {
        int minorFlowerSum = minorFlowers.Length;

        yield return new WaitForSeconds(firstGrowDelay);

        for (int x = 0; x < minorFlowerSum; x++)
        {
            minorFlowers[x].MinorFlowerGrow();
            yield return new WaitForSeconds(delayBetween);
        }
    }

}
