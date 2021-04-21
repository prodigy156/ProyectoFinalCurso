using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenrator : MonoBehaviour
{
    public static GridGenrator instance;


    public int width = 11;
    public int height = 7;
    public float offset = 1;

    public GameObject squarePrefab;

    public Transform gridReference;
    public Transform Grid;

    public GameObject[,] grid;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        instance = this;
        GridGenerator();
    }

    void GridGenerator()
    {
        grid = new GameObject[width, height];

        for(int y = 0; y < height; y++)
        {
            for(int x  = 0; x < width; x++)
            {
                float posX = x * offset;
                float posY = -y * offset;

                GameObject square = Instantiate(squarePrefab, gridReference.position + new Vector3(posX, posY, 0), Quaternion.identity) as GameObject;
                square.transform.SetParent(Grid);

                grid[x,y] = square;
            }
        }
    }
}
