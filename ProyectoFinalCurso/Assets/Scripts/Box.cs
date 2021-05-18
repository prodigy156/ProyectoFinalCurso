using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    bool ASide, lastSide;
    public Transform boxA, boxB;

    BoxCollider bc;

    Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        ASide = lastSide = true;

        bc = GetComponent<BoxCollider>();
    }

    public void SetPos(float distance)
    {
        bc.center = new Vector3(bc.center.x, bc.center.y, distance);
    }
}
