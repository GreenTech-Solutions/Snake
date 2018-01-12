using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Data;


public class SpawnFood : MonoBehaviour
{

    public GameObject foodPrefab;




    // Use this for initialization
    void Start () {
        // Генерировать еду каждые 4 секунды, начиная с третьей
		InvokeRepeating("Spawn",3,4);
	}

    void Spawn()
    {


        int x = (int)Random.Range(borderLeft.position.x,
            borderRight.position.x);

        int y = (int)Random.Range(borderTop.position.y,
            borderBottom.position.y);

        Instantiate(foodPrefab, new Vector2(x, y),
            Quaternion.identity);
    }

}
