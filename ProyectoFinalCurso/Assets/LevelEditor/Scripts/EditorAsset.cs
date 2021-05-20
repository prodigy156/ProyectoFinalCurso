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
        twoByFour,
        threeByTwo,
    }
    public AssetSize size;

    public enum Assets
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

    public Assets asset;
}
