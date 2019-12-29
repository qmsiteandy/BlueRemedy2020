using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlossomFlowers : MonoBehaviour {

    [Header("花")]
    private bool isGrowed = false;
    private Animator animator;

    [Header("長小花")]
    public float firstGrowDelay = 1f;
    public float delayBetween = 0.2f;
    public Transform childrenPlants_folder;
    private GroundPlants[] childPlants;

    [Header("特效")]
    public GameObject followPollenParticle;
    private ParticleSystem FlowerPollen_PS;
    private bool isParticlePlayed = false;
    public Transform particlePoint;

    [Header("燈光")]
    private Light flowerLight;
    private float initIntensity;
    private float initRange;

    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
        
        flowerLight = transform.GetChild(0).GetComponent<Light>();
        initIntensity = flowerLight.intensity; flowerLight.intensity = 0f;
        initRange = flowerLight.range; flowerLight.range = 0f;

        childPlants = new GroundPlants[childrenPlants_folder.childCount];
        for (int x = 0; x < childrenPlants_folder.childCount; x++)
        {
            childPlants[x] = childrenPlants_folder.GetChild(x).GetComponent<GroundPlants>();
            childPlants[x].b_isGrowFromMainFolwer = true;
        }

        FlowerPollen_PS = followPollenParticle.GetComponent<ParticleSystem>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!isGrowed) MainFlowerGrow();
        }
    }
    
    //主花生長
    void MainFlowerGrow()
    {
        isGrowed = true;
        animator.SetTrigger("grow");
        StartCoroutine(OpenLight(firstGrowDelay, 1.5f));
    }

    //animation呼叫
    void ThingAfterMainGrow()
    {
        if (!isParticlePlayed) FollowParticle();
        if (childPlants.Length > 0) StartCoroutine(ChildrenGrow());
    }

    void FollowParticle()
    {
        isParticlePlayed = true;

        GameObject particle = Instantiate(followPollenParticle, particlePoint.position, Quaternion.identity);
        Destroy(particle, 10f);
    }

    IEnumerator OpenLight(float delay, float inTime)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float addIntensity = initIntensity / inTime;
        float addRange = initRange / inTime;

        do{
            flowerLight.intensity += addIntensity * Time.deltaTime;
            flowerLight.range += addRange * Time.deltaTime;

            elapsed += Time.deltaTime;
            yield return null;
        }while (elapsed < inTime);

        flowerLight.intensity = initIntensity;
        flowerLight.range = initRange;
    }

    IEnumerator ChildrenGrow()
    {
        int childrenPlantsSum = childPlants.Length;

        yield return new WaitForSeconds(firstGrowDelay);

        for (int x = 0; x < childrenPlantsSum; x++)
        {
            childPlants[x].GrowByMainFlower();
            yield return new WaitForSeconds(delayBetween);
        }
    }

    public void Grow()
    {
        if (!isGrowed)
        {
            isGrowed = true;
            animator.SetTrigger("grow");
        }
    }
}
