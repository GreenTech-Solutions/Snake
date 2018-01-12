using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour
{

    Vector2 direction = Vector2.right;
    public GameObject tailPrefab;

    private float speed = 0.08f;
    private float defaultSpeed = 0.08f;


    List<Transform> tail = new List<Transform>();

    private bool ate = false;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("Move", 0.3f, speed);
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        if (xInput != 0 || yInput != 0)
        {
            if (xInput > 0) xInput = 1;
            else if (xInput < 0)
                xInput = -1;

            if (yInput > 0) yInput = 1;
            else if (yInput < 0)
                yInput = -1;


            if (xInput == yInput && xInput == 1)
            {
                speed *= 2;
            }
            else
            {
                speed = defaultSpeed;
            }

            direction = new Vector2(xInput, yInput);
        }



        //if (xInput != 0 || yInput != 0)
        //{
        //    direction = xInput != 0 ? (xInput / Mathf.Abs(xInput)) * Vector2.right
        //        : ((yInput / Mathf.Abs(yInput)) * Vector2.up);
        //}

    }

    void Move()
    {
        var v = transform.position;
        transform.Translate(direction);

        if (ate)
        {
            GameObject g = (GameObject)Instantiate(
                tailPrefab, v, Quaternion.identity);

            tail.Insert(0, g.transform);

            ate = false;
        }
        else if (tail.Count > 0)
        {
            tail.Last().position = v;

            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.StartsWith("Food"))
        {
            ate = true;

            Destroy(col.gameObject);
        }
        else
        {
            // ToDO LoseScreen
        }
    }
}
