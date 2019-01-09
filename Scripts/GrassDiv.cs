using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrassDiv : MonoBehaviour
{

    static float total_zone = 0f;
    static float grow_zone = 0f;
    static int percentNow = 0;
    static Text blossomPercent;

    public GameObject percentText;
    public List<GameObject> grassChildren;
    private bool isGrowed = false;
    private float zone_width;
    

    void Start()
    {
        if (percentText != null) blossomPercent = percentText.GetComponent<Text>();
        zone_width = GetComponent<BoxCollider2D>().size.x;
        total_zone += zone_width;

        for (int x = 0; x < transform.childCount; x++)
        {
            grassChildren.Add(transform.GetChild(x).gameObject);
        }

        foreach (GameObject child in grassChildren)
        {
            child.SetActive(false);
        }
    }
    public void GrassGrow()
    {
        if (!isGrowed)
        {
            foreach (GameObject child in grassChildren)
            {
                child.SetActive(true);
            }
            isGrowed = true;

            grow_zone += zone_width;

            int percent = (int)(grow_zone / total_zone * 100);
            StartCoroutine(changePercent(percent));
        }

    }

    static IEnumerator changePercent(int toPercent)
    {
        while (percentNow < toPercent)
        {
            percentNow += 1;
            blossomPercent.text = percentNow + "%";

            yield return null;
        }
    }
}
