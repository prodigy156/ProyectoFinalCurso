using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{
    public static EditorManager instance;

    public Camera cam;
    public LayerMask cellLayer;
    public Transform LevelAssets;

    [Header ("Asset Prefabs")]
    public GameObject Box1P;
    public GameObject LongBox1P;
    public GameObject FloorWall1P;


    bool instantiated = false;
    GameObject draggedAsset;
    GameObject unit;

    [Header("Cell Check")]
    //cell available check
    GameObject[,] grid;
    int gridHeight, gridWidth;
    GameObject pointedCell;
    bool canPlace;
    
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
        grid = GridGenrator.instance.grid;
        gridHeight = grid.GetLength(0);
        gridWidth = grid.GetLength(1);
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


        if(!instantiated)
        {
            draggedAsset = Instantiate(unit, Vector3.zero, Quaternion.identity) as GameObject;
            draggedAsset.transform.SetParent(LevelAssets);
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


    }

    /// <summary>
    /// AssignAssetToGrid is called when the user has pressed the mouse button while
    /// over the grid square
    /// </summary>
    public void AssignAssetToGrid()
    {
        draggedAsset.transform.position += new Vector3(0, 0, 0.1f);
        instantiated = false;
    }

    public void SetAssetSelected(int assetID)
    {
        if(currentAsset != (Assets)assetID)
        {
            Destroy(draggedAsset);
            instantiated = false;
            currentAsset = (Assets)assetID;
        }
        
    }

    void CheckCanPlace(GameObject cell)
    {
        int posY = -1;
        int posX = -1;

        for(int y = 0; y < gridHeight; y++)
        {
            for(int x = 0; x < gridWidth; x++)
            {
                if(grid[y, x].Equals(cell))
                {
                    posY= y;
                    posX= x;                    
                }
            }
        }
       
       
        if(!grid[posY, posX].gameObject.GetComponent<GridSquare>().available)
        {
            draggedAsset.GetComponent<SpriteRenderer>().material.color = Color.red;
        }
        else
        {
            draggedAsset.GetComponent<SpriteRenderer>().material.color = Color.white;
        }

    }
}
