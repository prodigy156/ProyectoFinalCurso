using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditorManager : MonoBehaviour
{
    public static EditorManager instance;

    public Camera cam;
    public LayerMask cellLayer;
    public LayerMask assetLayer;
    public Transform LevelSideA;
    public Transform LevelSideB;

    public Button saveButton;

    bool drawing = true;//true if asset selected, false if eraser selected

    [Header ("Asset Prefabs")]
    public GameObject InvisibleWall2P;
    public GameObject Box2P;
    public GameObject LongBox2P;
    public GameObject FloorWall2P;
    public GameObject Box1P;
    public GameObject LongBox1P;
    public GameObject FloorWall1P;
    public GameObject Key;
    public GameObject SpawnBox;

    public GameObject Door;
    public GameObject Portal;


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



    bool spawnBoxCreated, keyCreated, doorCreated;
    
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
        FloorWall2P,
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
        gridWidth = gridA.GetLength(0);
        gridHeight = gridA.GetLength(1);

        gridB = GridManager.instance.gridB;
        gridWidth = gridB.GetLength(0);
        gridHeight = gridB.GetLength(1);

        spawnBoxCreated = false;
        doorCreated = false;
        keyCreated = false;

        currentSide = Side.A;
        nextSide = currentSide;

        GridManager.instance.EnableCurrentDisableOtherGrid((int)currentSide);

        //CreateWalls();
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
            case Assets.FloorWall2P:
                unit = FloorWall2P;
                break;
            case Assets.Box1P:
                unit = Box1P;
                break;
            case Assets.Box2P:
                unit = Box2P;
                break;
            case Assets.LongBox1P:
                unit = LongBox1P;
                break;
            case Assets.LongBox2P:
                unit = LongBox2P;
                break;
            case Assets.Key:
                unit = Key;
                break;
            case Assets.SpawnBox:
                unit = SpawnBox;
                break;
            case Assets.Door:
                unit = Door;
                break;
            case Assets.Portal:
                unit = Portal;
                break;
            default:
            break;
        }
        Ray _ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(_ray.origin, _ray.direction * 1000f, Color.red);
        RaycastHit _hit;

            if (!instantiated)//if ther's no object to previsualize create one of the currents selected asset
            {
                draggedAsset = Instantiate(unit, Vector3.zero, Quaternion.identity) as GameObject;
                
                draggedAsset.GetComponent<SpriteRenderer>().enabled = false;
                instantiated = true;
            }

            if (Physics.Raycast(_ray, out _hit, 1000f, cellLayer))
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


        if(nextSide != currentSide)
        {
            GridManager.instance.EnableCurrentDisableOtherGrid((int)nextSide);
            if(nextSide == Side.A)
            {
                foreach(Transform child in LevelSideB)
                {
                    if(child.gameObject.GetComponent<EditorAsset>().twoP)
                    {
                        child.position = gridA[child.gameObject.GetComponent<EditorAsset>().opositeGridPosX , child.gameObject.GetComponent<EditorAsset>().gridPosY].gameObject.transform.position + new Vector3(0,0, -0.1f);
                    }
                    else
                    {
                        child.position = gridA[child.gameObject.GetComponent<EditorAsset>().opositeGridPosX, child.gameObject.GetComponent<EditorAsset>().gridPosY].gameObject.transform.position;
                    }
                }
                foreach (Transform child in LevelSideA)
                {
                    if (child.gameObject.GetComponent<EditorAsset>().twoP)
                    {
                        child.position = gridA[child.gameObject.GetComponent<EditorAsset>().gridPosX, child.gameObject.GetComponent<EditorAsset>().gridPosY].gameObject.transform.position + new Vector3(0, 0, -0.1f);
                    }
                    else
                    {
                        child.position = gridA[child.gameObject.GetComponent<EditorAsset>().gridPosX, child.gameObject.GetComponent<EditorAsset>().gridPosY].gameObject.transform.position + new Vector3(0, 0, -0.1f);
                    }
                }
            }
            else if (nextSide == Side.B)
            {
                foreach (Transform child in LevelSideA)
                {
                    if (child.gameObject.GetComponent<EditorAsset>().twoP)
                    {
                        child.position = gridB[child.gameObject.GetComponent<EditorAsset>().opositeGridPosX, child.gameObject.GetComponent<EditorAsset>().gridPosY].gameObject.transform.position + new Vector3(0, 0, -0.1f);
                    }
                    else
                    {
                        child.position = gridB[child.gameObject.GetComponent<EditorAsset>().opositeGridPosX, child.gameObject.GetComponent<EditorAsset>().gridPosY].gameObject.transform.position;
                    }
                }
                foreach (Transform child in LevelSideB)
                {
                    if (child.gameObject.GetComponent<EditorAsset>().twoP)
                    {
                        child.position = gridB[child.gameObject.GetComponent<EditorAsset>().gridPosX, child.gameObject.GetComponent<EditorAsset>().gridPosY].gameObject.transform.position + new Vector3(0, 0, -0.1f);
                    }
                    else
                    {
                        child.position = gridB[child.gameObject.GetComponent<EditorAsset>().gridPosX, child.gameObject.GetComponent<EditorAsset>().gridPosY].gameObject.transform.position + new Vector3(0, 0, -0.1f);
                    }
                }
            }

            currentSide = nextSide;
            if (currentSide == Side.A) { draggedAsset.transform.SetParent(LevelSideA); }
            else { draggedAsset.transform.SetParent(LevelSideB); }
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
        int opositePosX = -1;
        GameObject[,] grid;
        GameObject[,] opositeGrid;

        if(currentAsset == Assets.SpawnBox)
        {
            spawnBoxCreated = true;
            if(doorCreated)
            {
                saveButton.interactable = true;
            }
        }
        if(currentAsset == Assets.Door)
        {
            doorCreated = true;
            if(spawnBoxCreated)
            {
                saveButton.interactable = true;
            }
        }
        if(currentAsset == Assets.Key){ keyCreated = true;}

        if ( currentSide == Side.A)
        {
            draggedAsset.transform.SetParent(LevelSideA);
            grid = gridA;
            opositeGrid = gridB;
        }
        else
        {
            draggedAsset.transform.SetParent(LevelSideB);
            grid = gridB;
            opositeGrid = gridA;
        }

        for(int y = 0; y < gridHeight; y++)
        {
            for(int x = 0; x < gridWidth; x++)
            {
                if(grid[x, y].Equals(cell))
                {
                    posX= x;
                    posY= y;

                    opositePosX = gridWidth - (x+1);
                }
            }
        }
        switch ((int)draggedAsset.GetComponent<EditorAsset>().size) //disable the cells that the Asset will occupy
        {
            case 2:
                if (posX + 1 < gridWidth && posY + 1 < gridHeight)
                    opositePosX -= 1;
                break;
            case 3:
                if (posX + 3 < gridWidth && posY + 1 < gridHeight)
                    opositePosX -= 3;
                break;
        }
        switch ((int)draggedAsset.GetComponent<EditorAsset>().size) //disable the cells that the Asset will occupy
        {
            case 0:
                grid[posX, posY].gameObject.GetComponent<GridSquare>().available = false;
            break;
            case 1:
                grid[posX, posY].gameObject.GetComponent<GridSquare>().available = false;
                grid[posX, posY+1].gameObject.GetComponent<GridSquare>().available = false;
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
            case 4:
                grid[posX, posY].gameObject.GetComponent<GridSquare>().available = false;
                grid[posX, posY+1 ].gameObject.GetComponent<GridSquare>().available = false;
                grid[posX, posY+2 ].gameObject.GetComponent<GridSquare>().available = false;
                grid[posX+1, posY].gameObject.GetComponent<GridSquare>().available = false;
                grid[posX+1, posY+1].gameObject.GetComponent<GridSquare>().available = false;
                grid[posX+1, posY+2].gameObject.GetComponent<GridSquare>().available = false;
            break;
            default:
            break;
        }
        if (draggedAsset.GetComponent<EditorAsset>().twoP)
        {
            switch ((int)draggedAsset.GetComponent<EditorAsset>().size) //disable the cells that the Asset will occupy
            {
                case 0:
                    opositeGrid[opositePosX, posY].gameObject.GetComponent<GridSquare>().available = false;
                    break;
                case 1:
                    opositeGrid[opositePosX, posY].gameObject.GetComponent<GridSquare>().available = false;
                    opositeGrid[opositePosX, posY+1].gameObject.GetComponent<GridSquare>().available = false;
                    break;
                case 2:
                    opositeGrid[opositePosX, posY].gameObject.GetComponent<GridSquare>().available = false;
                    opositeGrid[opositePosX + 1, posY].gameObject.GetComponent<GridSquare>().available = false;
                    opositeGrid[opositePosX, posY + 1].gameObject.GetComponent<GridSquare>().available = false;
                    opositeGrid[opositePosX + 1, posY + 1].gameObject.GetComponent<GridSquare>().available = false;
                    break;
                case 3:
                    opositeGrid[opositePosX, posY].gameObject.GetComponent<GridSquare>().available = false;
                    opositeGrid[opositePosX + 1, posY].gameObject.GetComponent<GridSquare>().available = false;
                    opositeGrid[opositePosX + 2, posY].gameObject.GetComponent<GridSquare>().available = false;
                    opositeGrid[opositePosX + 3, posY].gameObject.GetComponent<GridSquare>().available = false;
                    opositeGrid[opositePosX, posY + 1].gameObject.GetComponent<GridSquare>().available = false;
                    opositeGrid[opositePosX + 1, posY + 1].gameObject.GetComponent<GridSquare>().available = false;
                    opositeGrid[opositePosX + 2, posY + 1].gameObject.GetComponent<GridSquare>().available = false;
                    opositeGrid[opositePosX + 3, posY + 1].gameObject.GetComponent<GridSquare>().available = false;
                    break;
                case 4:
                    opositeGrid[opositePosX, posY].gameObject.GetComponent<GridSquare>().available = false;
                opositeGrid[opositePosX, posY+1 ].gameObject.GetComponent<GridSquare>().available = false;
                opositeGrid[opositePosX, posY+2 ].gameObject.GetComponent<GridSquare>().available = false;
                opositeGrid[opositePosX+1, posY].gameObject.GetComponent<GridSquare>().available = false;
                opositeGrid[opositePosX+1, posY+1].gameObject.GetComponent<GridSquare>().available = false;
                opositeGrid[opositePosX+1, posY+2].gameObject.GetComponent<GridSquare>().available = false;
                    break;
                default:
                    break;
            }
        }

        if(currentSide == Side.A){ draggedAsset.GetComponent<EditorAsset>().sideA = true; }
        else { draggedAsset.GetComponent<EditorAsset>().sideA = false;}


        draggedAsset.transform.position += new Vector3(0, 0, 0.1f);
        draggedAsset.GetComponent<EditorAsset>().gridPosX = posX;
        draggedAsset.GetComponent<EditorAsset>().gridPosY = posY;
        draggedAsset.GetComponent<EditorAsset>().opositeGridPosX = opositePosX;

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
        int opositePosX = -1;
        GameObject[,] grid;
        GameObject[,] opositeGrid;

        if ( currentSide == Side.A)
        {
            grid = gridA;
            opositeGrid = gridB;
        }
        else
        {
            grid = gridB;
            opositeGrid = gridA;
        }

        for(int y = 0; y < gridHeight; y++)
        {
            for(int x = 0; x < gridWidth; x++)
            {
                if(grid[x, y].Equals(cell))
                {
                    posX= x;
                    posY= y;
                    opositePosX = gridWidth - (x + 1);
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
            case 1:
                if(posX < gridWidth &&  posY+1 < gridHeight)
                {
                    if(!grid[posX, posY].gameObject.GetComponent<GridSquare>().available||
                        !grid[posX, posY+1].gameObject.GetComponent<GridSquare>().available)
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
                    Debug.Log("fuera");
                }
            break;
            case 2:
                if(posX+1 < gridWidth &&  posY+1 < gridHeight)
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
                    Debug.Log("fuera");
                }
            break;
            case 3:
                if(posX+3 < gridWidth &&  posY+1 < gridHeight)
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
                    Debug.Log("fuera");
                }
            break;
            case 4:
                if(posX+1 < gridWidth &&  posY+2 < gridHeight)
                {
                    if(!grid[posX, posY].gameObject.GetComponent<GridSquare>().available ||
                    !grid[posX, posY+1 ].gameObject.GetComponent<GridSquare>().available ||
                    !grid[posX, posY+2 ].gameObject.GetComponent<GridSquare>().available ||
                    !grid[posX+1, posY].gameObject.GetComponent<GridSquare>().available ||
                    !grid[posX+1, posY+1].gameObject.GetComponent<GridSquare>().available ||
                    !grid[posX+1, posY+2].gameObject.GetComponent<GridSquare>().available)
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
                    Debug.Log("fuera");
                }
            break;
            default:
            break;
        }
        if(draggedAsset.GetComponent<EditorAsset>().twoP && canPlace)
        {
            switch ((int)draggedAsset.GetComponent<EditorAsset>().size) //Checks if the cells that the Asset will occupy are able
            {
                case 0:
                    if (!opositeGrid[opositePosX, posY].gameObject.GetComponent<GridSquare>().available)
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
                case 1:
                    if(posX < gridWidth &&  posY+1 < gridHeight)
                    {
                        if(!opositeGrid[opositePosX, posY].gameObject.GetComponent<GridSquare>().available||
                            !opositeGrid[opositePosX, posY+1].gameObject.GetComponent<GridSquare>().available)
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
                        Debug.Log("fuera");
                    }
                break;
                case 2:
                    opositePosX -= 1;
                    if (posX + 1 < gridWidth && posY + 1 < gridHeight)
                    {
                        if (!opositeGrid[opositePosX, posY].gameObject.GetComponent<GridSquare>().available ||
                        !opositeGrid[opositePosX + 1, posY].gameObject.GetComponent<GridSquare>().available ||
                        !opositeGrid[opositePosX, posY + 1].gameObject.GetComponent<GridSquare>().available ||
                        !opositeGrid[opositePosX + 1, posY + 1].gameObject.GetComponent<GridSquare>().available)
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
                        Debug.Log("fuera");
                    }
                    break;
                case 3:
                    opositePosX -= 3;
                    if (posX + 3 < gridWidth && posY + 1 < gridHeight)
                    {
                        if (!opositeGrid[opositePosX, posY].gameObject.GetComponent<GridSquare>().available ||
                        !opositeGrid[opositePosX + 1, posY].gameObject.GetComponent<GridSquare>().available ||
                        !opositeGrid[opositePosX + 2, posY].gameObject.GetComponent<GridSquare>().available ||
                        !opositeGrid[opositePosX + 3, posY].gameObject.GetComponent<GridSquare>().available ||
                        !opositeGrid[opositePosX, posY + 1].gameObject.GetComponent<GridSquare>().available ||
                        !opositeGrid[opositePosX + 1, posY + 1].gameObject.GetComponent<GridSquare>().available ||
                        !opositeGrid[opositePosX + 2, posY + 1].gameObject.GetComponent<GridSquare>().available ||
                        !opositeGrid[opositePosX + 3, posY + 1].gameObject.GetComponent<GridSquare>().available)
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
                        Debug.Log("fuera");
                    }
                    break;
                case 4:
                    opositePosX -= 1;
                    if(posX+1 < gridWidth &&  posY+2 < gridHeight)
                {
                    if(!opositeGrid[opositePosX, posY].gameObject.GetComponent<GridSquare>().available ||
                    !opositeGrid[opositePosX, posY+1 ].gameObject.GetComponent<GridSquare>().available ||
                    !opositeGrid[opositePosX, posY+2 ].gameObject.GetComponent<GridSquare>().available ||
                    !opositeGrid[opositePosX+1, posY].gameObject.GetComponent<GridSquare>().available ||
                    !opositeGrid[opositePosX+1, posY+1].gameObject.GetComponent<GridSquare>().available ||
                    !opositeGrid[opositePosX+1, posY+2].gameObject.GetComponent<GridSquare>().available)
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
                        Debug.Log("fuera");
                    }
                    break;
                default:
                    break;
            }
        }
        if(currentSide == Side.B && currentAsset == Assets.SpawnBox || currentSide == Side.B && currentAsset == Assets.Door)
        {
            draggedAsset.GetComponent<SpriteRenderer>().material.color = Color.red;
            canPlace = false;
        }
        if(currentAsset == Assets.SpawnBox && spawnBoxCreated)
        {
            draggedAsset.GetComponent<SpriteRenderer>().material.color = Color.red;
            canPlace = false;
        }
        else if(currentAsset == Assets.Door && doorCreated)
        {
            draggedAsset.GetComponent<SpriteRenderer>().material.color = Color.red;
            canPlace = false;
        }
        else if(currentAsset == Assets.Key && keyCreated)
        {
            draggedAsset.GetComponent<SpriteRenderer>().material.color = Color.red;
            canPlace = false;
        }
    }

    public void ChangeSide()
    {
        if(currentSide == Side.A)
        {
            nextSide = Side.B;
        }
        else if(currentSide == Side.B)
        {
            nextSide = Side.A;
        }
    }

    void CreateWalls()
    {
        GameObject asset;
        asset = Instantiate(InvisibleWall2P, gridA[0,0].transform.position + new Vector3(-0.5f, 0.5f, 0), Quaternion.identity);
        asset.transform.SetParent(LevelSideA);
        for(int i  = 0; i < gridWidth +1; i++)
        {
            asset = Instantiate(InvisibleWall2P, gridA[0,0].transform.position + new Vector3(0.5f * i, 0.5f, 0), Quaternion.identity);
            asset.transform.SetParent(LevelSideA);
        }

        for(int i  = 0; i < gridHeight; i++)
        {
            asset = Instantiate(InvisibleWall2P, gridA[0,0].transform.position + new Vector3(-0.5f, 0.5f * -i, 0), Quaternion.identity);
            asset.transform.SetParent(LevelSideA);
        }

        for(int i  = 0; i < gridHeight; i++)
        {
            asset = Instantiate(InvisibleWall2P, gridA[gridWidth-1,0].transform.position + new Vector3(0.5f, 0.5f * -i, 0), Quaternion.identity);
            asset.transform.SetParent(LevelSideA);
        }

        asset = Instantiate(InvisibleWall2P, gridA[0, gridHeight - 1].transform.position + new Vector3(0.5f, -0.5f, 0), Quaternion.identity);
        asset.transform.SetParent(LevelSideA);
        for(int i  = 0; i < gridWidth +1; i++)
        {
            asset = Instantiate(InvisibleWall2P, gridA[0,gridHeight - 1].transform.position + new Vector3(0.5f * i, -0.5f, 0), Quaternion.identity);
            asset.transform.SetParent(LevelSideA);
        }
    }

    public void ExitLevelEditor()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);

    }


}