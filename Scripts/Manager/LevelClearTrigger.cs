using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelClearTrigger : MonoBehaviour {

    private bool isClear = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(isClear == false)
            {
                Clear();
            }
        }
    }

    void Clear()
    {
        PlayerStatus.canControl = false;
        isClear = true;

        GameObject.Find("GameManager").GetComponent<GameManager>().LevelClear(SceneManager.GetActiveScene().buildIndex);
        transform.Find("LevelClear_Canvas").gameObject.SetActive(true);
    }
}
