using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAsset : MonoBehaviour
{
    public int posY;
    public int posX;
    public int twoP;
    public int sideA;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValues()
    {
        Vector3 position = new Vector3(posX, posY * -1, 0);

        transform.position = position;
    }
}
