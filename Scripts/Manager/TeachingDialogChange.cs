using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachingDialogChange : MonoBehaviour {

    public bool isRandom = false;

    private GameObject[] TeachingDialogObj;
    private CanvasGroup canvasGroup;
    private bool hasChange = true;
    private int dialogIndexNow = 0;

    //某些教學石碑會隨機跳某一個內容

    void Start ()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        TeachingDialogObj = new GameObject[this.transform.childCount];
        for (int x = 0; x < transform.childCount; x++)
        {
            TeachingDialogObj[x] = transform.GetChild(x).gameObject;
            TeachingDialogObj[x].SetActive(false);
        }
        TeachingDialogObj[dialogIndexNow].SetActive(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (canvasGroup.alpha == 0 && !hasChange)
        {
            if (isRandom) RandomOneAndOpen();
            else NextOneAndOpen();

            hasChange = true;
        }
        else if(canvasGroup.alpha > 0.7f && hasChange)
        {
            hasChange = false;
        }
    }

    void NextOneAndOpen()
    {
        TeachingDialogObj[dialogIndexNow].SetActive(false);

        dialogIndexNow += 1;
        if (dialogIndexNow >= TeachingDialogObj.Length) dialogIndexNow = 0;

        TeachingDialogObj[dialogIndexNow].SetActive(true);
    }

    void RandomOneAndOpen()
    {
        int newIndex;
        do
        {
            newIndex = Random.Range(0, TeachingDialogObj.Length);
        }
        while (newIndex == dialogIndexNow);

        TeachingDialogObj[newIndex].SetActive(true);
        dialogIndexNow = newIndex;
    }
}
