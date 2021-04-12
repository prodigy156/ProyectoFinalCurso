using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenrator : MonoBehaviour
{
    public int width = 11;
    public int height = 7;
    public int offset = 1;

    public GameObject squarePrefab;

    public Transform gridReference;
    public Transform Grid;

    GameObject[,] grid;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        GridGenerator();
    }
    
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        
    }
    
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
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
