using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptLoadEditor : MonoBehaviour
{
    public MenuManager menuManager;


    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            menuManager.LoadEditor();
        }
    }

    void Start()
    {
    }

}