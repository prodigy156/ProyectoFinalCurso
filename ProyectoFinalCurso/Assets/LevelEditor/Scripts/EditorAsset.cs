using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorAsset : MonoBehaviour
{
    public int gridPosY, gridPosX, opositeGridPosX;
    public int sizeY, sizeX;
    public bool twoP;
    public bool sideA;

    public enum AssetSize
    {
        oneByOne,
        twoByOne,
        twoByTwo,
        twoByFour
    }
    public AssetSize size;

}
