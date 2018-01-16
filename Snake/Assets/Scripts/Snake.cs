using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    Vector2 direction = Vector2.right;
    public GameObject tailPrefab;

    private AudioSource source;



    public AudioClip AppleBiting;
    public AudioClip Ouch;
    private float speed = 0.1f;

    public Text Position;


    public static List<Transform> tail = new List<Transform>();

    public static Transform Head;

    private bool ate = false;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("Move", 0.3f, speed);
        Head = transform;

    }

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown(KeyCode.W))
            {
                direction = Vector2.up;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                direction = Vector2.down;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                direction = Vector2.left;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                direction = Vector2.right;
            }
    }

    bool IsInt(object some)
    {
        int intv = Convert.ToInt32(some);
        double doublev = Convert.ToDouble(some);

        return intv==doublev;
    }

    void Move()
    {
        var v = transform.position;



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

        v.x = (int)v.x;
        v.y = (int)v.y;

        if (v.x >= 14 && direction == Vector2.right)
        {
            v.x = -15;
            transform.position = v;
        }
        else if (v.x <= -15 && direction == Vector2.left)
        {
            v.x = 14;
            transform.position = v;

        }
        else if (v.y >= 15 && direction == Vector2.up)
        {
            v.y = -14;
            transform.position = v;
        }
        else if (v.y <= -14 && direction == Vector2.down)
        {
            v.y = 15;
            transform.position = v;
        }
        else
        {
            transform.Translate(direction);
        }

        Position.text = v.ToString();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.StartsWith("Food"))
        {
            source.PlayOneShot(AppleBiting, 1);
            ate = true;

            Destroy(col.gameObject);
        }
        else if (col.gameObject.name == "Rock")
        {
            source.PlayOneShot(Ouch,1);
            // ToDO LoseScreen
        }
    }
}
