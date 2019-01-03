using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTest : MonoBehaviour {
    public GameObject frontGrass, backGrass;
    public Transform Grasses;
    List<Vector3> frontGrassPositions = new List<Vector3>(); //陣列儲存前草的位置
    List<Vector3> backGrassPositions = new List<Vector3>();  //陣列儲存後草的位置

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    public void SpawnGrass() {
        Vector3 newPos = transform.position - new Vector3(0,0.5f,0); //草原始位置在主角中心，所以要下移一點
        bool tooClose = false; //預設草與草之間沒有太靠近

        if (Random.Range(0, 101) > 50) //變數決定長前草還是後草
        {
            foreach (Vector3 temp in frontGrassPositions)
            {
                Vector3 diff = temp - newPos;
                if (Vector2.SqrMagnitude(new Vector2(diff.x, diff.y)) <= 0.16f)
                {
                    tooClose = true;
                    break;
                }
            }
            if (!tooClose)
            {
                Instantiate(frontGrass, newPos, Quaternion.identity,Grasses);
                frontGrassPositions.Add(newPos);
            }
        }
        else {
            foreach (Vector3 temp in backGrassPositions)
            {
                Vector3 diff = temp - newPos;
                if (Vector2.SqrMagnitude(new Vector2(diff.x, diff.y)) <= 0.16f)
                {
                    tooClose = true;
                    break;
                }
            }
            if (!tooClose)
            {
                Instantiate(backGrass, newPos, Quaternion.identity, Grasses);
                backGrassPositions.Add(newPos);
            }
        }

    }
}
