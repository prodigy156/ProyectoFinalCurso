using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour
{
    public static EditorManager instance;

    public bool isDragging = false;
    public Camera cam;
    public LayerMask cellLayer;
    public GameObject unit;

    bool instantiated = false;
    GameObject draggedAsset;
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDragging)
        {
            if(!instantiated)
            {
                draggedAsset = Instantiate(unit, Vector3.zero, Quaternion.identity);
                instantiated = true;
            }

            Ray _ray = cam.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(_ray.origin, _ray.direction * 1000f, Color.red);
            RaycastHit _hit;

            if(Physics.Raycast(_ray, out _hit, 1000f, cellLayer))
            {
                draggedAsset.transform.position = _hit.collider.transform.position + new Vector3(0, 0, -0.1f);
            }
        }
        else
        {
            instantiated = false;
        }
    }
}
