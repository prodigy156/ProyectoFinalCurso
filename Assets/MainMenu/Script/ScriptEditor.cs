using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptEditor : MonoBehaviour
{
    public MenuManager menuManager;

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            menuManager.EditorLevel();
        }
    }
}
