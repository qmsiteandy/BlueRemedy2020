using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour {

    public int waterEnergyMax = 200;
    private int waterEnergy;
    public int dirtMax;
    private int dirt = 0;

    private float chargeDelay = 1f;
    private int waterPerCharge = 3;
    private float elapsed = 0f;

    //private SpriteRenderer[] playerSprite= { null, null, null };
    private UI_Manager UI_manager;

    private Material dirtyRippeMat;

    void Start ()
    {
        waterEnergy = waterEnergyMax;
        dirtMax = waterEnergyMax;

        //for (int x = 0; x < 3; x++) playerSprite[x] = transform.GetChild(x).GetComponent<SpriteRenderer>();
        //for (int x = 0; x < 3; x++) Oka[x] = transform.GetChild(x).gameObject;
        UI_manager = GameObject.Find("UI_Canvas").GetComponent<UI_Manager>();

        dirtyRippeMat = this.transform.Find("DirtyRipple").GetComponent<SpriteRenderer>().material;
        dirtyRippeMat.SetFloat("_drityDegree", 0);
    }

    void Update()
    {
        if (elapsed > chargeDelay) { ModifyWaterEnergy(waterPerCharge); elapsed = 0f; }
        elapsed += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Y)) { ModifyDirt(10); ModifyWaterEnergy(-10); }
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
        dirtyRippeMat.SetFloat("_drityDegree", dirtyDegree);
    }

    public void ConnectNewLevelUI()
    {
        UI_manager = GameObject.Find("UI_Canvas").GetComponent<UI_Manager>();
    }
}
