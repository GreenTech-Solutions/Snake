using UnityEngine;
using System.Collections;

public class MyMap : MonoBehaviour
{

    public Sprite[] img;

    public int IndexImg = 0;

    private int CurrentIndex = 0;

    /// <summary>
    /// Смена картинки на блоке
    /// </summary>
    void Changeimg()
    {
        if (CurrentIndex != IndexImg)
        {
            int ListSize = img.Length;

            if (ListSize > IndexImg)
            {
                SpriteRenderer S = this.GetComponent<SpriteRenderer>();

                S.sprite = img[IndexImg];

                CurrentIndex = IndexImg;
            }
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Changeimg();
	
	}
}
