using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour {

    public int waterEnergyMax = 200;
    private int waterEnergy;
    public int dirtMax;
    private int dirt = 0;

    public float chargeDelay = 1f;
    public int waterPerCharge = 3;
    private float elapsed = 0f;

    //private SpriteRenderer[] playerSprite= { null, null, null };
    private UI_Manager UI_manager;

    public GameObject dirtyRipple;
    private Material dirtyRippleMat;


    void Start ()
    {
        waterEnergy = waterEnergyMax;
        dirtMax = waterEnergyMax;

        //for (int x = 0; x < 3; x++) playerSprite[x] = transform.GetChild(x).GetComponent<SpriteRenderer>();
        //for (int x = 0; x < 3; x++) Oka[x] = transform.GetChild(x).gameObject;
        UI_manager = GameObject.Find("UI_Canvas").GetComponent<UI_Manager>();

        UI_manager = GameObject.Find("UI_Canvas").GetComponent<UI_Manager>();

        dirtyRippleMat = dirtyRipple.GetComponent<SpriteRenderer>().material;
        dirtyRippleMat.SetFloat("_dirtyRippleMat", 0f);
    }

    void Update()
    {
        if (elapsed > chargeDelay) { ModifyWaterEnergy(waterPerCharge); elapsed = 0f; }
        elapsed += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Y)) { dirt += 10; waterEnergy -= 10; }
    }

    public void ModifyWaterEnergy(int amount)
    {
        waterEnergy += amount;
        if (waterEnergy > waterEnergyMax) waterEnergy = waterEnergyMax;
        else if (waterEnergy < 0) waterEnergy = 0;

        SetDirtyDegree();

        UI_manager.SetWaterUI(waterEnergy);

        if (waterEnergy == 0) ; //GameOver
    }

    public void ModifyDirt(int amount)
    {
        dirt += amount;
        if (dirt > dirtMax) dirt = dirtMax;
        else if (dirt < 0) dirt = 0;

        SetDirtyDegree();
    }

    void SetDirtyDegree()
    {
        float dirtyDegree;
        dirtyDegree = (float)dirt / (dirt + waterEnergy);

        UI_manager.SetDirtyUI(dirtyDegree);

        dirtyRippleMat.SetFloat("_drityDegree", dirtyDegree);
    }
}
