using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewElement : MonoBehaviour
{
    public bool isDragging = false;
    
    public void SetDragging(bool _isDragging)
    {
        isDragging = _isDragging;
        EditorManager.instance.isDragging = _isDragging;
    }
}
