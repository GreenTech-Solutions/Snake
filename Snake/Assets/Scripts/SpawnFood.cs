using System.Collections;
using System.Collections.Generic;
using Tiled2Unity;
using UnityEngine;
using static Data;


public class SpawnFood : MonoBehaviour
{

    public GameObject foodPrefab;

    public TiledMap map;

    private int left, right, top, bottom;

    // Use this for initialization
    void Start () {
        // Генерировать еду начиная с x каждые y секунды
        InvokeRepeating("Spawn",0,2);

	}

    void Spawn()
    {
        int x = (int)Random.Range(-15,
            14);

        int y = (int)Random.Range(-14,
            15);

        var v = new Vector3(x,y);

        foreach (var coord in Snake.tail)
        {
            if (coord.position == v)
            {
                Spawn();
            }
        }

        if (v == Snake.Head.position)
        {
            Spawn();
        }


        Instantiate(foodPrefab, new Vector2(x, y),
            Quaternion.identity);
    }

}
