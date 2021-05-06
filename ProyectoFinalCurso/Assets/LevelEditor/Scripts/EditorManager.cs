using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{
    public static EditorManager instance;

    public Camera cam;
    public LayerMask cellLayer;
    public Transform LevelSideA;
    public Transform LevelSideB;

    [Header ("Asset Prefabs")]
    public GameObject Box1P;
    public GameObject LongBox1P;
    public GameObject FloorWall1P;


    bool instantiated = false;
    GameObject draggedAsset;
    GameObject unit;

    [Header("Grid")]
    public bool aSide = true;
    public bool canPlace;
    
    GameObject[,] gridA;
    GameObject[,] gridB;
    int gridHeight, gridWidth;
    GameObject pointedCell;
    
     
    public EditorAsset editorAsset;

    
    enum Side
    {
        A,
        B
    }
    Side currentSide;
    Side nextSide;

    enum Assets
    {
        SpawnBox,
        Medal,
        Box1P,
        Box2P,
        LongBox1P,
        LongBox2P,
        Portal,
        Key,
        Button,
        Stairs,
        ResetButton,
        Rock1P,
        Rock2P,
        Rope,
        Door,
        Keydoor,
        FloorWall1P,
        FloorWall2P
    }
    [SerializeField]
    Assets currentAsset;


    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        unit = FloorWall1P;
        instance = this;
        gridA = GridManager.instance.gridA;
        gridHeight = gridA.GetLength(0);
        gridWidth = gridA.GetLength(1);

        gridB = GridManager.instance.gridB;
        gridHeight = gridB.GetLength(0);
        gridWidth = gridB.GetLength(1);

        currentSide = Side.A;
        nextSide = currentSide;

        GridManager.instance.EnableCurrentDisableOtherGrid((int)currentSide);
    }

    // Update is called once per frame
    void Update()
    {
        //set unit as the actual asset selected prefab
        switch (currentAsset)
        {
            case Assets.FloorWall1P:
            unit = FloorWall1P;
            break;
            case Assets.Box1P:
            unit = Box1P;
            break;
            case Assets.LongBox1P:
            unit = LongBox1P;
            break;
            default:
            break;
        }


        if(!instantiated)//if ther's no object to previsualize create one of the currents selected asset
        {
            draggedAsset = Instantiate(unit, Vector3.zero, Quaternion.identity) as GameObject;
            if(currentSide == Side.A){ draggedAsset.transform.SetParent(LevelSideA);}
            else { draggedAsset.transform.SetParent(LevelSideB); }
            draggedAsset.GetComponent<SpriteRenderer>().enabled = false;
            instantiated = true;
        }

        Ray _ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(_ray.origin, _ray.direction * 1000f, Color.red);
        RaycastHit _hit;

        if(Physics.Raycast(_ray, out _hit, 1000f, cellLayer))
        {
            draggedAsset.GetComponent<SpriteRenderer>().enabled = true;
            draggedAsset.transform.position = _hit.collider.transform.position + new Vector3(0, 0, -0.2f);

            pointedCell = _hit.transform.gameObject;
            CheckCanPlace(pointedCell);
        }
        else
        {
            draggedAsset.GetComponent<SpriteRenderer>().enabled = false;
        }

        if(aSide) { nextSide = Side.A;}
        else { nextSide = Side.B;}

        if(nextSide != currentSide)
        {
            GridManager.instance.EnableCurrentDisableOtherGrid((int)nextSide);
            if(nextSide == Side.A)
            {
                foreach(Transform child in LevelSideB)
                {
                    if(child.gameObject.GetComponent<EditorAsset>().twoP)
                    {
                        //child.position = new Vector3(
                    }
                }
            }

            currentSide = nextSide;
        }
    }

    /// <summary>
    /// AssignAssetToGrid is called when the user has pressed the mouse button while
    /// over the grid square
    /// </summary>
    public void AssignAssetToGrid(GameObject cell)
    {
        int posX = -1;
        int posY = -1;
        GameObject[,] grid;

        if( currentSide == Side.A)
        {
            grid = gridA;
        }
        else
        {
            grid = gridB;
        }

        for(int y = 0; y < gridHeight; y++)
        {
            for(int x = 0; x < gridWidth; x++)
            {
                if(grid[y, x].Equals(cell))
                {
                    posX= y;
                    posY= x;                    
                }
            }
        }
        switch ((int)draggedAsset.GetComponent<EditorAsset>().size) //disable the cells that the Asset will occupy
        {
            case 0:
                grid[posX, posY].gameObject.GetComponent<GridSquare>().available = false;
            break;
            case 2:
                grid[posX, posY].gameObject.GetComponent<GridSquare>().available = false;
                grid[posX+1, posY].gameObject.GetComponent<GridSquare>().available = false;
                grid[posX, posY+1].gameObject.GetComponent<GridSquare>().available = false;
                grid[posX+1, posY +1].gameObject.GetComponent<GridSquare>().available = false;
            break;
            case 3:
                grid[posX, posY].gameObject.GetComponent<GridSquare>().available = false;
                grid[posX+1, posY ].gameObject.GetComponent<GridSquare>().available = false;
                grid[posX+2, posY ].gameObject.GetComponent<GridSquare>().available = false;
                grid[posX+3, posY ].gameObject.GetComponent<GridSquare>().available = false;
                grid[posX, posY+1].gameObject.GetComponent<GridSquare>().available = false;
                grid[posX+1, posY+1].gameObject.GetComponent<GridSquare>().available = false;
                grid[posX+2, posY+1].gameObject.GetComponent<GridSquare>().available = false;
                grid[posX+3, posY+1].gameObject.GetComponent<GridSquare>().available = false;
            break;
            default:
            break;
        }

        if(currentSide == Side.A){ draggedAsset.GetComponent<EditorAsset>().sideA = true; }
        else { draggedAsset.GetComponent<EditorAsset>().sideA = false;}


        draggedAsset.transform.position += new Vector3(0, 0, 0.1f);
        draggedAsset.GetComponent<EditorAsset>().gridPosX = posX;
        draggedAsset.GetComponent<EditorAsset>().gridPosY = posY;
        instantiated = false;
    }

    /// <summary>
    /// SetAssetSelected is called when the user has pressed the mouse button while
    /// over an asset button. It changes the current asset selected to put in the grid
    /// </summary>
    public void SetAssetSelected(int assetID)
    {
        if(currentAsset != (Assets)assetID)
        {
            Destroy(draggedAsset);
            instantiated = false;
            currentAsset = (Assets)assetID;
        }
    }

    void CheckCanPlace(GameObject cell )
    {
        int posX = -1;
        int posY = -1;
        GameObject[,] grid;

        if( currentSide == Side.A)
        {
            grid = gridA;
        }
        else
        {
            grid = gridB;
        }

        for(int y = 0; y < gridHeight; y++)
        {
            for(int x = 0; x < gridWidth; x++)
            {
                if(grid[y, x].Equals(cell))
                {
                    posX= y;
                    posY= x;                    
                }
            }
        }

        switch ((int)draggedAsset.GetComponent<EditorAsset>().size) //Checks if the cells that the Asset will occupy are able
        {
            case 0:
                if(!grid[posX, posY].gameObject.GetComponent<GridSquare>().available)
                {
                    draggedAsset.GetComponent<SpriteRenderer>().material.color = Color.red;
                    canPlace = false;
                }
                else
                {
                    draggedAsset.GetComponent<SpriteRenderer>().material.color = Color.white;
                    canPlace = true;
                }
            break;
            case 2:
                if(posX+1 < gridHeight &&  posY+1 < gridWidth)
                {
                    if(!grid[posX, posY].gameObject.GetComponent<GridSquare>().available||
                    !grid[posX+1, posY].gameObject.GetComponent<GridSquare>().available||
                    !grid[posX, posY+1].gameObject.GetComponent<GridSquare>().available||
                    !grid[posX+1, posY +1].gameObject.GetComponent<GridSquare>().available)
                    {
                        draggedAsset.GetComponent<SpriteRenderer>().material.color = Color.red;
                        canPlace = false;
                    }
                    else
                    {
                        draggedAsset.GetComponent<SpriteRenderer>().material.color = Color.white;
                        canPlace = true;
                    }
                }
                else 
                {
                    draggedAsset.GetComponent<SpriteRenderer>().material.color = Color.red;
                    canPlace = false;
                }
            break;
            case 3:
                if(posX+3 < gridHeight &&  posY+1 < gridWidth)
                {
                    if(!grid[posX, posY].gameObject.GetComponent<GridSquare>().available ||
                    !grid[posX+1, posY ].gameObject.GetComponent<GridSquare>().available ||
                    !grid[posX+2, posY ].gameObject.GetComponent<GridSquare>().available ||
                    !grid[posX+3, posY ].gameObject.GetComponent<GridSquare>().available ||
                    !grid[posX, posY+1].gameObject.GetComponent<GridSquare>().available ||
                    !grid[posX+1, posY+1].gameObject.GetComponent<GridSquare>().available ||
                    !grid[posX+2, posY+1].gameObject.GetComponent<GridSquare>().available ||
                    !grid[posX+3, posY+1].gameObject.GetComponent<GridSquare>().available)
                    {
                        draggedAsset.GetComponent<SpriteRenderer>().material.color = Color.red;
                        canPlace = false;
                    }
                    else
                    {
                        draggedAsset.GetComponent<SpriteRenderer>().material.color = Color.white;
                        canPlace = true;
                    }
                }
                else 
                {
                    draggedAsset.GetComponent<SpriteRenderer>().material.color = Color.red; 
                    canPlace = false;
                }
            break;
            default:
            break;
        }


    }
}