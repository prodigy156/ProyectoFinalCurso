using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptOptions : MonoBehaviour
{
    public MenuManager menuManager;

    new Transform transform;


    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            menuManager.OptionGame();
        }
    }

    void Start()
    {
    }

}