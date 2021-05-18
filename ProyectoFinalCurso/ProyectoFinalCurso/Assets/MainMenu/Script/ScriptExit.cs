using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptExit : MonoBehaviour
{
    public MenuManager menuManager;


    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            menuManager.QuitGame();
        }

    }


}