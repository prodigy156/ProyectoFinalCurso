using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;


    public int width = 11;
    public int height = 7;
    public float offset = 1;

    public GameObject squarePrefab;

    public Transform gridReference;
    public Transform GridA;
    public Transform GridB;

    public GameObject[,] gridA;
    public GameObject[,] gridB;

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
        gridA = new GameObject[width, height];

        for(int y = 0; y < height; y++)
        {
            for(int x  = 0; x < width; x++)
            {
                float posX = x * offset;
                float posY = -y * offset;

                GameObject square = Instantiate(squarePrefab, gridReference.position + new Vector3(posX, posY, 0), Quaternion.identity) as GameObject;
                square.transform.SetParent(GridA);

                gridA[x,y] = square;
            }
        }
        
        gridB = new GameObject[width, height];

        for(int y = 0; y < height; y++)
        {
            for(int x  = 0; x < width; x++)
            {
                float posX = x * offset;
                float posY = -y * offset;

                GameObject square = Instantiate(squarePrefab, gridReference.position + new Vector3(posX, posY, 0), Quaternion.identity) as GameObject;
                square.transform.SetParent(GridB);

                gridB[x,y] = square;
            }
        }
    }

    public void EnableCurrentDisableOtherGrid(int side)
    {

        if(side == 0)
        {
            GridA.gameObject.SetActive(true);
            GridB.gameObject.SetActive(false);
            Debug.Log("BDisabled" + side);
        }
        else if(side == 1)
        {
            GridB.gameObject.SetActive(true);
            GridA.gameObject.SetActive(false);
            Debug.Log("ADisabled" + side);
        }


    }
}