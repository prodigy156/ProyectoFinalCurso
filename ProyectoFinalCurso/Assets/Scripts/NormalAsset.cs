using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAsset : MonoBehaviour
{
    public int posY;
    public int posX;
    public float posZ;
    public int twoP;
    public int sideA;

    public void SetValues()
    {
        Vector3 position = new Vector3(posX, posY * -1, posZ);

        transform.position = position;
    }
}
