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

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        EditorManager.instance.OnAssetDeleted(this.gameObject);
        Destroy(this.gameObject);
    }
}
