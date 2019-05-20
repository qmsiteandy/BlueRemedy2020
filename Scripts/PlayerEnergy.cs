using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour {

    public int waterEnergyMax = 10;
    private int waterEnergy;
    private int dirtMax;
    private int dirt = 0;
    private float dirtyDegree;

    private SpriteRenderer[] playerSprite= { null, null, null }; 


    void Start ()
    {
        waterEnergy = waterEnergyMax;
        dirtMax = waterEnergyMax;
        dirtyDegree = (float)dirt / (dirt + waterEnergy);

        for (int x = 0; x < 3; x++) playerSprite[x] = transform.GetChild(x).GetComponent<SpriteRenderer>();
    }

    void SetDirtyDegree()
    {
        dirtyDegree = (float)dirt / (dirt + waterEnergy);
        Debug.Log("dirtyDegree" + dirtyDegree);
    }

    void SetPlayerSpriteColor()
    {
        for (int x = 0; x < 3; x++) playerSprite[x].color = new Color(1 - dirtyDegree, 1 - dirtyDegree, 1 - dirtyDegree);
    }

    public void ModifyWaterEnergy(int amount)
    {
        waterEnergy += amount;
        if (waterEnergy > waterEnergyMax) waterEnergy = waterEnergyMax;
        else if (waterEnergy < waterEnergyMax) waterEnergy = 0;

        SetDirtyDegree();
        SetPlayerSpriteColor();

        if (waterEnergy == 0) ; //GameOver
    }

    public void ModifyDirt(int amount)
    {
        dirt += amount;
        if (dirt > dirtMax) dirt = dirtMax;
        else if (dirt < 0) dirt = 0;

        SetDirtyDegree();
        SetPlayerSpriteColor();
    }
}
