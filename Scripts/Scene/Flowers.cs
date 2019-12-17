using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flowers : MonoBehaviour {

    [Header("花")]
    public bool isMainFolower = false;
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
    public float maxSwinOffset = 0.45f;
    public float maxSwinSpeed = 0.14f;
    public float swinbackSpeed = 0.08f;
    private float swinToAngle, swinSpeed;
    private float xOffset=0f;
    private Material material;


    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
        material = GetComponent<SpriteRenderer>().material;
        
        if (isMainFolower)
        {
            flowerLight = transform.GetChild(0).GetComponent<Light>();
            flowerLight.enabled = false;

            minorFlowers = new Flowers[minorFlowers_folder.childCount];
            for (int x = 0; x < minorFlowers_folder.childCount; x++) minorFlowers[x] = minorFlowers_folder.GetChild(x).GetComponent<Flowers>();
        } 
    }

    void Update()
    {
        if (xOffset != swinToAngle)
        {
            xOffset = Mathf.Lerp(xOffset, swinToAngle, swinSpeed);
            if (Mathf.Abs(xOffset - swinToAngle) < 0.01f) xOffset = swinToAngle;
            material.SetFloat("_xOffset", xOffset);
        }
        else{ swinToAngle = 0f; swinSpeed=swinbackSpeed; }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!isGrowed && isMainFolower) MainFlowerGrow();
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Mathf.Abs(PlayerControl.xSpeed) >= 4f)
            {
                float speedRate = PlayerControl.xSpeed / PlayerControl.speedLimit;
                swinToAngle = speedRate * maxSwinOffset;
                swinSpeed = Mathf.Abs(speedRate * maxSwinSpeed);
            }
            else { swinToAngle = 0f; swinSpeed = swinbackSpeed; }
        }
    }
    
    //主花生長
    void MainFlowerGrow()
    {
        isGrowed = true;
        animator.SetTrigger("grow");
    }

    //animation呼叫
    void ThingAfterMainGrow()
    {
        if (!isMainFolower) return;

        flowerLight.enabled = true;
        if (isMainFolower && !isParticlePlayed) FollowParticle();
        if (minorFlowers.Length > 0) StartCoroutine(MinorGrow());
    }

    void FollowParticle()
    {
        isParticlePlayed = true;

        GameObject particle = Instantiate(FollowGrowParticle, particlePoint.position, Quaternion.identity);
        Destroy(particle, 5f);
    }

    IEnumerator MinorGrow()
    {
        int minorFlowerSum = minorFlowers.Length;

        yield return new WaitForSeconds(firstGrowDelay);

        for (int x = 0; x < minorFlowerSum; x++)
        {
            minorFlowers[x].MinorFlowerGrow();
            yield return new WaitForSeconds(delayBetween);
        }
    }

    public void MinorFlowerGrow()
    {
        if (!isGrowed)
        {
            isGrowed = true;
            animator.SetTrigger("grow");
        }
    }
}
