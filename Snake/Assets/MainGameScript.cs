using UnityEngine;
using System.Collections;

public class MainGameScript : MonoBehaviour
{
    public float speedSnake = 100f;

    private float RenderTime = 0,CurrentTime ;

    public GameObject Block;

    GameObject[,] MapArea = new GameObject[30,20];

    struct  Body
    {
        public int IndexX, IndexY;
    }

    Body[]  Snake = new Body[600];

    private int SizeSnake = 0;

    private int Dx = 0;
    int Dy = 0;

    void InputKey()
    {
        float x = Input.GetAxis("Horizontal");
        if (x > 0)
        {
            Dx = 1;
            Dy = 0;
        }
        else if (x < 0)
        {
            Dx = -1;
            Dy = 0;
        }

        float y = Input.GetAxis("Vertical");

        if (y > 0)
        {
            Dx = 0;
            Dy = -1;
        }
        else if (y < 0)
        {
            Dx = 0;
            Dy = 1;
        }
    }

    void movesnake()

    {

        //если указано направление для движения то двигаемся

        if ((Dx != 0) || (Dy != 0))

        {

            int L = SizeSnake - 1;

            if (L > 0)
            {
                SetColor(MapArea[Snake[L].IndexX,Snake[L].IndexY],0);
            }

            //смещаем змею с хвоста в сторону головы

            for (int I = SizeSnake - 1; I > 0; I--) Snake[I] = Snake[I - 1];

            //двигаем голову в новую точку

            Snake[0].IndexX += Dx;

            Snake[0].IndexY -= Dy;

            //если мы вылезли на граници поля то 2 варианат 1 мы вылезаем с другой стороны либо умераем

            //Проверяем координаты по оси X

            if (Snake[0].IndexX > 29) Snake[0].IndexX = 0;

            if (Snake[0].IndexX < 0) Snake[0].IndexX = 29;

            //проверяем по оси Y

            if (Snake[0].IndexY > 19) Snake[0].IndexY = 0;

            if (Snake[0].IndexY < 0) Snake[0].IndexY = 19;

            TestStep();

        }


    }

    void CreateEat()
    {
        bool Insert_Apple = true;

        int Coordx, Coordy;

        while (Insert_Apple)
        {
            Coordx = Random.Range(0, 29);
            Coordy = Random.Range(0, 19);

            Debug.Log("X" + Coordx + " Y"+ Coordy);

            int color = GetColor(MapArea[Coordx, Coordy]);

            if (color == 0)
            {
                SetColor(MapArea[Coordx, Coordy],3);
                Insert_Apple = false;
            }
        }
    }

    void TestStep()
    {
        int Headx = Snake[0].IndexX;
        int Heady = Snake[0].IndexY;

        int Step = GetColor(MapArea[Headx, Heady]);

        switch (Step)
        {
            case 3:
                GrowSnake();
                CreateEat();
                break;
            
        }
    }

    /// <summary>
    /// Генерация карты
    /// </summary>
    void GenMap()
    {
        Vector3 PointStart = new Vector3(0,0,0);

        for (int Y = 0; Y < 20; Y++)
        {
            for (int X = 0; X < 30; X++)
            {
                PointStart = new Vector3(X, Y, 0);
                MapArea[X, Y] = (GameObject) Instantiate(Block, PointStart, Quaternion.identity);
            }
        }
    }

    int GetColor(GameObject O)
    {
        MyMap ComponentObject = O.GetComponent<MyMap>();

        int Imgcolor = ComponentObject.IndexImg;

        return Imgcolor;
    }

    void SetColor(GameObject O, int Set_color)
    {
        MyMap ComponentObject = O.GetComponent<MyMap>();

        ComponentObject.IndexImg = Set_color;
    }

    void GrowSnake()
    {
        if (SizeSnake == 0)
        {
            Snake[SizeSnake].IndexX = 10;
            Snake[SizeSnake].IndexY = 10;
        }
        else
        {
            Snake[SizeSnake].IndexX = Snake[SizeSnake - 1].IndexX;

            Snake[SizeSnake].IndexY = Snake[SizeSnake - 1].IndexY;
        }

        SizeSnake++;
    }

    void DrawSnake()
    {
        for (int i = 1; i < SizeSnake; i++)
        {
            SetColor(MapArea[Snake[i].IndexX, Snake[i].IndexY], 2);
        }

        if (SizeSnake > 0)
        {
            SetColor(MapArea[Snake[0].IndexX,Snake[0].IndexY], 1);
        }
    }

	// Use this for initialization
	void Start ()
	{
	    speedSnake = 0.1f;

	    Vector3 CameraPose = new Vector3(14.5f,9.5f,-10);
	    this.transform.position = CameraPose;

	    Camera C = this.GetComponent<Camera>();
	    C.orthographicSize = 10;

        GenMap();

	    for (int i = 0; i < 2; i++)
	    {
	        GrowSnake();
	    }

	    for (int i = 0; i < 10; i++)
	    {
	        CreateEat();
	    }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
        DrawSnake();

        InputKey();

	    CurrentTime = Time.time;

	    float alpha = CurrentTime - RenderTime;

        if (alpha > speedSnake)
        {
            movesnake();

            RenderTime = Time.time;
        }


    }
}
