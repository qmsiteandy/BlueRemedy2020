using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnThisEnemyDead : MonoBehaviour {

    private Enemy_Dead enemy_Dead;

    public UnityEvent unityEvent;

    // Use this for initialization
    void Start ()
    {
        enemy_Dead = GetComponent<Enemy_Dead>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (enemy_Dead.isDead)
        {
            StartCoroutine(AfterEnemyDead(0.5f));
        }
    }

    IEnumerator AfterEnemyDead(float delay)
    {
        yield return new WaitForSeconds(delay);

        unityEvent.Invoke();
    }


}
