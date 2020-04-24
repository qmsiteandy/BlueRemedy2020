using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead_Menu : MonoBehaviour {

    private bool canContinue = false;

    private void OnEnable()
    {
        canContinue = false;
        GetComponent<Animator>().SetTrigger("Dead");
    }

    private void Update()
    {
        if (canContinue)
        {
            if (Input.GetButtonDown("Submit"))
            {
                canContinue = false;

                this.gameObject.SetActive(false);
                GameObject.Find("GameManager").GetComponent<GameManager>().GoToScene("Level_Room");

                this.enabled = false;
            }
        }
    }

    void CanContinue()
    {
        canContinue = true;
    }
}
