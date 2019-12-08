using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenManager : MonoBehaviour {

    public GameObject[] gameObjects;

    //將UI類母物件於遊戲開始時SetActive
	void Start () {
		foreach(GameObject Obg in gameObjects)
        {
            Obg.SetActive(true);
        }
	}

}
