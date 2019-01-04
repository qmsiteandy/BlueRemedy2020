using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSkill : MonoBehaviour {

    public PlayerControl playerControl;
    public FruitManager fruitManager;

    [Header("1.Attack")]
    public float throwSpeed = 15f;
    public Transform throwPoint;
    public GameObject poisonApple;

    [Header("2.Defence")]
    public GameObject defenceShield;
    public float D_DurabilityMax = 300;
    public int duraLoseRate = 35;
    private SpriteRenderer D_sprite;
    private bool isDefensing = false;
    private float D_Durability = 0;
    private float timer = 0f;

    [Header("3.Heal")]
    public GameObject HealObj;
    public Transform rotatePoint;
    private bool isHealing = false;


    void Start()
    {
        D_sprite = defenceShield.GetComponent<SpriteRenderer>();

        defenceShield.SetActive(false);
        HealObj.SetActive(false);
        gameObject.SetActive(false);
    }

   

    void Update () {

        if (Input.GetButtonDown("Attack"))
        {
            GameObject newPoiApple = Instantiate(poisonApple, throwPoint.position, throwPoint.transform.rotation);

            Rigidbody2D rigid = newPoiApple.GetComponent<Rigidbody2D>();
            if (playerControl.facingRight) rigid.velocity = throwSpeed * throwPoint.transform.right;
            else rigid.velocity = -1 * throwSpeed * throwPoint.transform.right;

            fruitManager.FirstLoseFresh(FruitmanData.InfoList[1].attack_freshLoss);
        }


        if (Input.GetButtonDown("Defence"))
        {
            defenceShield.SetActive(true);
            D_Durability = D_DurabilityMax;

            isDefensing = true;
            playerControl.isShielded = true;
            fruitManager.FirstLoseFresh(FruitmanData.InfoList[1].defence_freshLoss);
        }
        if (isDefensing) ShieldState();


        if (Input.GetButtonDown("Heal"))
        {
            if(!isHealing) StartCoroutine(HealDrink());
        }

    }


    void ShieldState()
    {
        D_Durability -= Time.deltaTime * duraLoseRate;

        if (D_Durability < 0)
        {
            isDefensing = false;
            playerControl.isShielded = false;

            defenceShield.SetActive(false);

            return;
        }

        timer = (timer <= 0) ? 0 : timer - Time.deltaTime;
        if (timer == 0)
        {
            timer = (D_Durability / D_DurabilityMax) * 2;
            StartCoroutine(FlashOnce());
        } 
    }

    IEnumerator FlashOnce()
    {
        D_sprite.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        D_sprite.color = new Color(1f, 1f, 1f, 1f);
    }


    void ShieldDamage(int damage)
    {
        D_Durability -= damage;
    }

    IEnumerator HealDrink()
    {
        int maxAngle = 75;
        int rotateAngle = 5;
        int totalFrame = maxAngle / rotateAngle;
        int frame = 0;

        isHealing = true;

        playerControl.canMove = false;
        HealObj.SetActive(true);

        if (!playerControl.facingRight) { rotateAngle = -rotateAngle; }

        while (frame < totalFrame)
        {
            HealObj.transform.RotateAround(rotatePoint.position, Vector3.forward, rotateAngle);
            frame += 1;
            yield return null;
        }
        while (frame > 0)
        {
            HealObj.transform.RotateAround(rotatePoint.position, Vector3.forward, -rotateAngle);
            frame -= 1;
            yield return null;
        }

        FruitmanBase firstFruitBase = fruitManager.firstOne.GetComponent<FruitmanBase>();

        int healValue = (int)(FruitmanData.InfoList[1].heal * firstFruitBase.fresh / firstFruitBase.freshMax);
        playerControl.Heal(healValue);

        HealObj.SetActive(false);
        playerControl.canMove = true;

        isHealing = false;

        fruitManager.FirstLoseFresh(10000);
    }
}
