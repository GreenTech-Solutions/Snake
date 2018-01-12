using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour {

    public Transform BorderTop;
    public Transform BorderBottom;
    public Transform BorderLeft;
    public Transform BorderRight;

    public static Transform borderTop;
    public static Transform borderBottom;
    public static Transform borderLeft;
    public static Transform borderRight;

    // Use this for initialization
    void Start ()
    {
        borderTop = BorderTop;
        borderBottom = BorderBottom;
        borderLeft = BorderLeft;
        borderRight = BorderRight;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
