using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour {

    public int waterEnergyMax = 200;
    private int waterEnergy;
    private int dirtMax;
    private int dirt = 0;
    private float dirtyDegree;

    public float chargeDelay = 1f;
    public int waterPerCharge = 3;
    private float elapsed = 0f;

    private SpriteRenderer[] playerSprite= { null, null, null };

    private UI_Manager UI_manager;


    void Start ()
    {
        waterEnergy = waterEnergyMax;
        dirtMax = waterEnergyMax;
        dirtyDegree = (float)dirt / (dirt + waterEnergy);

        for (int x = 0; x < 3; x++) playerSprite[x] = transform.GetChild(x).GetComponent<SpriteRenderer>();
        UI_manager = GameObject.Find("UI_Canvas").GetComponent<UI_Manager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) { ModifyWaterEnergy(-10); }

        if (elapsed > chargeDelay) { ModifyWaterEnergy(waterPerCharge); elapsed = 0f; }
        elapsed += Time.deltaTime;

        Debug.Log("waterEnergy" + waterEnergy);
    }

    void SetDirtyDegree()
    {
        dirtyDegree = (float)dirt / (dirt + waterEnergy);
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
}
