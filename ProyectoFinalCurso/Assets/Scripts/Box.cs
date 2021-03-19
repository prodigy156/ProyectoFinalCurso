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
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != lastPos)
            lastPos = transform.position;

        if (ASide != lastSide)
        {
            if (lastSide)
            {
                bc.center = new Vector3(bc.center.x, bc.center.y, boxB.position.z);
            }
            else
            {
                bc.center = new Vector3(bc.center.x, bc.center.y, boxA.position.z);
            }
        }
    }

    public void TellSide(bool _ASide)
    {
        ASide = _ASide;
    }
}
